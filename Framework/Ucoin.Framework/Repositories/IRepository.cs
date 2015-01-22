using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;

namespace Ucoin.Framework.Repositories
{
    public interface IRepository<T, TKey> : IReadOnlyRepository<T, TKey>, IDisposable 
        where T : class, IAggregateRoot<TKey>
    {
        IRepositoryContext Context { get; }

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(TKey id);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        bool Exists(Expression<Func<T, bool>> predicate);
    }
}
