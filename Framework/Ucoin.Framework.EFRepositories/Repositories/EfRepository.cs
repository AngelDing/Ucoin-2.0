using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Specifications;
using Ucoin.Framework.CompareObjects;
using Ucoin.Framework.SqlDb.Entities;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Diagnostics;

namespace Ucoin.Framework.SqlDb.Repositories
{
    public class EfRepository<T, Tkey> : Repository<T, Tkey>, IEfRepository<T, Tkey>
        where T : EfEntity<Tkey>, IAggregateRoot<Tkey>
    {
        private readonly IEfRepositoryContext efContext;
        private readonly DbContext db;
        protected RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy> RetryPolicy { get; private set; }

        public EfRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEfRepositoryContext)
            {
                this.efContext = context as IEfRepositoryContext;
                this.db = efContext.DbContext;
            }

            var incremental = new Incremental(5, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1.5))
            {
                FastFirstRetry = true
            };
            this.RetryPolicy = new RetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>(incremental);

            this.RetryPolicy.Retrying += (s, e) =>
                Trace.TraceWarning("An error occurred in attempt number {1} to access the database in ConferenceService: {0}",
                e.LastException.Message, e.CurrentRetryCount);
        }

        #region IRepository

        protected override void DoInsert(T entity)
        {
            efContext.RegisterNew(entity);
        }

        protected override void DoInsert(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                efContext.RegisterNew(e);
            }
        }

        protected override void DoUpdate(T entity)
        {
            efContext.RegisterModified(entity);
        }

        protected override void DoUpdate(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                efContext.RegisterModified(e);
            }
        }

        protected override void DoDelete(Tkey key)
        {
            var deleteEntity = this.DoGetByKey(key);
            efContext.RegisterDeleted(deleteEntity);
        }

        protected override void DoDelete(T entity)
        {
            efContext.RegisterDeleted(entity);
        }

        protected override void DoDelete(Expression<Func<T, bool>> predicate)
        {
            var deleteList = this.GetBy(predicate);
            foreach (var e in deleteList)
            {
                efContext.RegisterDeleted(e);
            }
        }

        protected override bool DoExists(Expression<Func<T, bool>> predicate)
        {
            var count = this.RetryPolicy.ExecuteAction(() => GetSet().Count(predicate));
            return count != 0;
        }

        #endregion

        #region IReadOnlyRepository

        protected override T DoGetByKey(Tkey key)
        {
            return this.RetryPolicy.ExecuteAction(
                () => GetSet().FirstOrDefault(p => (object)p.Id == (object)key));
        }

        protected override IEnumerable<T> DoGetAll()
        {
            return this.RetryPolicy.ExecuteAction(() => GetSet());
        }

        protected override IEnumerable<T> DoGetBy(Expression<Func<T, bool>> predicate)
        {
            return this.RetryPolicy.ExecuteAction(() => GetSet().Where(predicate));
        }

        protected override IEnumerable<T> DoGetBy(ISpecification<T> spec)
        {
            return this.RetryPolicy.ExecuteAction(() => GetSet().Where(spec.SatisfiedBy()));
        }

        #endregion

        #region IEFRepository

        public DbContext DbContext
        {
            get
            {
                if (db == null)
                {
                    throw new ArgumentException("DbContext should not be null!");
                }
                return db;
            }
        }

        #endregion 

        private IDbSet<T> GetSet()
        {
            return efContext.DbContext.Set<T>();
        }
    }
}
