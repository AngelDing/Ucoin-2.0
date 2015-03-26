using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Text;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.SqlDb.Repositories
{
    public class EfRepositoryContext : RepositoryContext, IEfRepositoryContext
    {
        private readonly DbContext dbContext;

        public EfRepositoryContext(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.OnDispose(disposing);
        }

        public DbContext DbContext
        {
            get { return this.dbContext; }
        }

        #region IRepositoryContext Members

        public override void RegisterNew<T>(T obj)
        {
            this.dbContext.Entry(obj).State = EntityState.Added;
        }

        public override void RegisterModified<T>(T obj)
        {            
            this.dbContext.ApplyChanges(obj);
        }

        public override void RegisterDeleted<T>(T obj)
        {
            this.dbContext.Entry(obj).State = EntityState.Deleted;
        }

        #endregion

        #region IUnitOfWork

        public override void Commit()
        {
            //此處手動進行相關邏輯的校驗，故需要全局關閉SaveChanges時自動的校驗：
            //Configuration.ValidateOnSaveEnabled = false;
            var errors = dbContext.GetValidationErrors();

            if (errors.Any())
            {
                var errorMsgs = GetErrors(errors);
                throw new EfRepositoryException(errorMsgs);
            }

            dbContext.SaveChanges();
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
