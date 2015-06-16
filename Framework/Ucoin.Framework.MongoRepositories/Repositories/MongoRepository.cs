using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ucoin.Framework.Specifications;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.MongoDb.Entities;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Validator;

namespace Ucoin.Framework.MongoDb.Repositories
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
        public IRepositoryContext RepoContext
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IReadOnlyRepository

        public T GetByKey(TKey id)
        {            
            return this.Collection.Find<T>(p => p.Id.ToString() == id.ToString()).FirstOrDefaultAsync().Result;
        }

        public IEnumerable<T> GetAll()
        {
            return this.Collection.Find(new BsonDocument()).ToListAsync().Result;
        }

        public IEnumerable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return this.Collection.Find<T>(predicate).ToListAsync().Result;
        }

        public IEnumerable<T> GetBy(ISpecification<T> specification)
        {
            return this.GetBy(specification.SatisfiedBy());
        }

        #endregion

        #region IRepository

        public virtual void Insert(T entity)
        {
            Validate(entity);
            this.Collection.InsertOneAsync(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            Validate(entities);
            this.Collection.InsertManyAsync(entities);
        }

        public void Update(T entity)
        {
            Validate(entity);
            this.Collection.ReplaceOneAsync<T>(p => p.Id.ToString() == entity.Id.ToString(), entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            Validate(entities);
            foreach (T entity in entities)
            {
                this.Update(entity);
            }
        }

        public void Delete(TKey id)
        {
            this.Collection.DeleteOneAsync<T>(p => p.Id.ToString() == id.ToString());
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
            var obj = this.Collection.Find<T>(predicate).SingleOrDefaultAsync().Result;
            return obj != null;
        }

        #endregion

        #region IMongoRepository

        //public IQueryable<T> CollectionQueryable
        //{
        //    get
        //    {
        //        return this.Collection.AsQueryable<T>();
        //    }
        //}

        public void Update(Expression<Func<T, bool>> query, Dictionary<string, object> columnValues)
        {
            if (columnValues == null || columnValues.Count == 0)
            {
                throw new ArgumentException("Update Columns is Null!", "columnValues");
            }

            var updates = new List<UpdateDefinition<T>>();
            var builder = Builders<T>.Update;
            foreach (var key in columnValues.Keys)
            {
                var definition = builder.Set(key, columnValues[key]);
                updates.Add(definition);
            }
            var update = builder.Combine(updates);
            this.Collection.UpdateOneAsync<T>(query, update);           
        }

        public void Update(Expression<Func<T, bool>> query, T entity)
        {
            Update(query, entity.NeedUpdateList);
        }

        public void RemoveAll()
        {
            this.Collection.DeleteManyAsync(p => true);
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
                //this.Collection.Database.Disconnect();
            }
            //http://docs.mongodb.org/ecosystem/tutorial/getting-started-with-csharp-driver/#getting-started-with-csharp-driver
            //The C# driver has a connection pool to use connections to the server efficiently. 
            //There is no need to call Connect or Disconnect; 
            //just let the driver take care of the connections (calling Connect is harmless, 
            //but calling Disconnect is bad because it closes all the connections in the connection pool).
        }

        #endregion

        private void Validate(T entity)
        {
            var validator = new EntityValidator();
            if (validator.IsValid(entity) == false)
            {
                throw new UcoinValidationException(validator.GetInvalidMessages());
            }
        }

        private void Validate(IEnumerable<T> entities)
        {
            var validator = new EntityValidator();
            var errorList = new List<string>();
            foreach (var e in entities)
            {
                if (validator.IsValid(e) == false)
                {
                    errorList.AddRange(validator.GetInvalidMessages());
                }
            }
            if (errorList.Any())
            {
                throw new UcoinValidationException(errorList);
            }
        }
    }
}
