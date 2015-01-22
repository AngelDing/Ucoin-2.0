using Ucoin.Framework.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Repositories
{
    public abstract class Repository<T, TKey> : IRepository<T, TKey> 
        where T : class, IAggregateRoot<TKey>
    {
        private readonly IRepositoryContext context;

        public Repository(IRepositoryContext context)
        {
            this.context = context;
        }

        public IRepositoryContext Context
        {
            get { return context; }
        }

        #region Abstract Methods

        public virtual void Dispose()
        {
            context.Dispose();
        }

        protected abstract void DoInsert(T entity);

        protected abstract void DoInsert(IEnumerable<T> entities);

        protected abstract void DoUpdate(T entity);

        protected abstract void DoUpdate(IEnumerable<T> entities);

        protected abstract void DoDelete(TKey id);

        protected abstract void DoDelete(T entity);

        protected abstract void DoDelete(Expression<Func<T, bool>> predicate);

        protected abstract bool DoExists(Expression<Func<T, bool>> predicate);

        protected abstract T DoGetByKey(TKey key);

        protected abstract IEnumerable<T> DoGetAll();

        protected abstract IEnumerable<T> DoGetBy(Expression<Func<T, bool>> predicate);

        protected abstract IEnumerable<T> DoGetBy(ISpecification<T> spec);

        #endregion

        #region IRepository

        public void Insert(T entity)
        {
            DoInsert(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            DoInsert(entities);
        }

        public void Update(T entity)
        {
            DoUpdate(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            DoUpdate(entities);
        }      

        public void Delete(TKey id)
        {
            DoDelete(id);
        }

        public void Delete(T entity)
        {
            DoDelete(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            DoDelete(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return DoExists(predicate);
        }

        public T GetByKey(TKey key)
        {
            return DoGetByKey(key);
        }

        public IEnumerable<T> GetAll()
        {
            return DoGetAll();
        }

        public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return DoGetBy(predicate);
        }

        public IEnumerable<T> GetBy(ISpecification<T> spec)
        {
            return DoGetBy(spec);
        }
       
        #endregion        
    }
}
