using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Specifications;
using Ucoin.Framework.Repositories;

namespace Ucoin.Framework.MongoRepository
{
    public class MongoRepository<T, TKey> : BaseMongoDB<T>, IMongoRepository<T, TKey>
        where T : BaseMongoEntity, IAggregateRoot<TKey>
    {
        //public WriteConcern MongoWriteConcern { get; set; }

        public MongoRepository(string connectionString)
            : base(connectionString)
        {
        }

        #region IRepositoryContext

        /// <summary>
        /// 由于MongoDB暂不支持事务，且大部分操作是原子性的，故暂不实现UnitOfWork模式
        /// </summary>
        public IRepositoryContext Context
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IReadOnlyRepository

        public T GetByKey(TKey id)
        {
            if (typeof(T).IsSubclassOf(typeof(StringKeyMongoEntity)))
            {
                return this.GetById(new ObjectId(id as string));
            }

            return this.Collection.FindOneByIdAs<T>(BsonValue.Create(id));
        }

        private T GetById(ObjectId id)
        {
            return this.Collection.FindOneByIdAs<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return this.Collection.FindAll();
        }

        public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return CollectionQueryable.Where(predicate);
        }

        public IEnumerable<T> GetBy(ISpecification<T> specification)
        {
            return CollectionQueryable.Where(specification.SatisfiedBy());
        }

        #endregion

        #region IRepository

        public virtual void Insert(T entity)
        {
            this.Collection.Insert<T>(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            this.Collection.InsertBatch<T>(entities);
        }

        public void Update(T entity)
        {
            this.Collection.Save<T>(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                this.Collection.Save<T>(entity);
            }
        }

        public void Delete(TKey id)
        {
            if (typeof(T).IsSubclassOf(typeof(StringKeyMongoEntity)))
            {
                this.Collection.Remove(Query.EQ("_id", new ObjectId(id as string)));
            }
            else
            {
                this.Collection.Remove(Query.EQ("_id", BsonValue.Create(id)));
            }
        }

        public void Delete(T entity)
        {
            this.Delete(entity.Id);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var dList = this.GetBy(predicate).ToList();
            foreach (T entity in dList)
            {
                this.Delete(entity.Id);
            }
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return CollectionQueryable.Any(predicate);
        }

        #endregion

        #region IMongoRepository

        public IQueryable<T> CollectionQueryable
        {
            get
            {
                return this.Collection.AsQueryable<T>();
            }
        }

        public void Update(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues)
        {
            if (columnValues == null || columnValues.Count == 0)
            {
                throw new ArgumentException("Update Columns is Null!", "columnValues");
            }
            var mongoQuery = Query<T>.Where(query);
            var update = new UpdateBuilder();
            columnValues.Keys.ToList().ForEach(x => update.SetWrapped(x, columnValues[x]));

            this.Collection.Update(mongoQuery, update);           
        }

        public void Update(Expression<Func<T, bool>> query, T entity)
        {
            Update(query, entity.NeedUpdateList);
        }

        public void RemoveAll()
        {
            this.Collection.RemoveAll();
        }

        #endregion             

        #region IDispose

        public void Dispose()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 顯式釋放MongoDB的鏈接，默認不釋放
        /// </summary>
        /// <param name="isDispose">true：需要顯式釋放鏈接</param>
        public void Dispose(bool isDispose = false)
        {
            //顯式釋放所有的MongoDB鏈接，目前用於主要用於Task
            if (isDispose == true)
            {
                this.Collection.Database.Server.Disconnect();
            }
            //http://docs.mongodb.org/ecosystem/tutorial/getting-started-with-csharp-driver/#getting-started-with-csharp-driver
            //The C# driver has a connection pool to use connections to the server efficiently. 
            //There is no need to call Connect or Disconnect; 
            //just let the driver take care of the connections (calling Connect is harmless, 
            //but calling Disconnect is bad because it closes all the connections in the connection pool).
        }

        #endregion
    }
}
