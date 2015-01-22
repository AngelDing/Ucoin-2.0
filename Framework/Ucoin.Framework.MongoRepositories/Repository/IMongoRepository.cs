
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.Specifications;

namespace Ucoin.Framework.MongoRepository
{
    public interface IMongoRepository<T, TKey> : IRepository<T, string>
        where T : class, IAggregateRoot<string>
    {
        IQueryable<T> CollectionQueryable { get; }

        T GetByKey(TKey id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 規約模式：规约的主要作用是沟通，在.net中，规约的实现可以直接引用Lambda Expression，
        /// 具体见上面的GetBy方法，不仅实现简单，而且还能直接使用到ORM上以减小数据库查询开销。
        /// 对于其它的面向对象解决方案而言，规约的实现就不一定那么直观，
        /// 另外MongoDB不支持可空判斷的表達式，如：Where(p => (param.Id == null || param.Id.Value == p.Id))
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        IEnumerable<T> GetBy(ISpecification<T> specification);

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(TKey id);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        bool Exists(Expression<Func<T, bool>> predicate);

        void Update(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues);

        void Update(Expression<Func<T, bool>> query, T entity);

        void RemoveAll();
    }
}
