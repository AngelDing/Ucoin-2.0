using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.EFRepository
{
    public class EFRepositoryContext : RepositoryContext, IEFRepositoryContext
    {
        private readonly DbContext efContext;

        public EFRepositoryContext(DbContext efContext)
        {
            this.efContext = efContext;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                efContext.Dispose();
            }
            base.Dispose(disposing);
        }

        public DbContext Context
        {
            get { return this.efContext; }
        }

        #region IRepositoryContext Members

        public override void RegisterNew(object obj)
        {
            this.efContext.Entry(obj).State = EntityState.Added;
        }

        public override void RegisterModified(object obj)
        {
            this.efContext.Entry(obj).State = EntityState.Modified;
        }

        public override void RegisterDeleted(object obj)
        {
            this.efContext.Entry(obj).State = EntityState.Deleted;
        }

        #endregion

        #region IUnitOfWork

        public override void Commit()
        {
            //此處手動進行相關邏輯的校驗，故需要全局關閉SaveChanges時自動的校驗：
            //Configuration.ValidateOnSaveEnabled = false;
            var errors = efContext.GetValidationErrors();

            if (errors.Any())
            {
                var errorMsgs = GetErrors(errors);
                throw new EFRepositoryException(errorMsgs);
            }

            efContext.SaveChanges();
        }

        private string GetErrors(IEnumerable<DbEntityValidationResult> results)
        {
            var errorMsgs = new StringBuilder();
            int counter = 0;

            foreach (DbEntityValidationResult result in results)
            {
                counter++;
                errorMsgs.AppendFormat("Failed Object #{0}: Type is {1}", counter, result.Entry.Entity.GetType().Name);
                errorMsgs.AppendLine();
                errorMsgs.AppendFormat(" Number of Problems: {0}", result.ValidationErrors.Count);
                errorMsgs.AppendLine();
                foreach (DbValidationError error in result.ValidationErrors)
                {
                    errorMsgs.AppendFormat(" - {0}", error.ErrorMessage);
                    errorMsgs.AppendLine();
                }
            }

            return errorMsgs.ToString();
        }

        #endregion
    }
}
