using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity;
using Ucoin.Framework.Repositories;
using Ucoin.Framework.Entities;
using Ucoin.Framework.Specifications;
using Ucoin.Framework.CompareObjects;

namespace Ucoin.Framework.EFRepository
{
    public class EFRepository<T, Tkey> : Repository<T, Tkey>, IEFRepository<T, Tkey>
        where T : EFEntity<Tkey>, IAggregateRoot<Tkey>
    {
        private readonly IEFRepositoryContext efContext;
        private readonly DbContext db;       

        public EFRepository(IRepositoryContext context)
            : base(context)
        {
            if (context is IEFRepositoryContext)
            {
                this.efContext = context as IEFRepositoryContext;
                this.db = efContext.DbContext;
            }
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
            var count = GetSet().Count(predicate);
            return count != 0;
        }

        #endregion

        #region IReadOnlyRepository

        protected override T DoGetByKey(Tkey key)
        {
            return GetSet().FirstOrDefault(p => (object)p.Id == (object)key);
        }

        protected override IEnumerable<T> DoGetAll()
        {
            return GetSet();
        }

        protected override IEnumerable<T> DoGetBy(Expression<Func<T, bool>> predicate)
        {
            return GetSet().Where(predicate);
        }

        protected override IEnumerable<T> DoGetBy(ISpecification<T> spec)
        {
            return GetSet().Where(spec.SatisfiedBy());
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

        /// <summary>
        /// 更新聚合實體，同時新增，修改或者刪除相關聯有變動的子表信息
        /// </summary>
        /// <param nameproperty="entity">聚合實體</param>
        /// <param name="compareResult">新舊對象比較結果</param>
        public void FullUpdate(T entity, ComparisonResult compareResult)
        {
            //entity.NeedUpdateList = compareResult.GetDifferentPropertys();
            this.Update(entity);

            foreach (var add in compareResult.NeedAddList.Keys)
            {
                foreach (var e in compareResult.NeedAddList[add])
                {
                    this.DbContext.Entry(e).State = EntityState.Added;
                }
            }
            foreach (var delete in compareResult.NeedDeleteList.Keys)
            {
                foreach (var e in compareResult.NeedDeleteList[delete])
                {
                    this.DbContext.Entry(e).State = EntityState.Deleted;
                }
            }
            foreach (var updateList in compareResult.NeedUpdateList.Values)
            {
                foreach(var update in updateList)
                {
                    this.DbContext.ApplyChanges(update as BaseEntity);
                    //this.Update(update);
                }                
            }
            //foreach (var update in compareResult.NeedUpdateList.Keys)
            //{
            //    foreach (var e in compareResult.NeedUpdateList[update])
            //    {
            //        this.DbContext.Entry(e).State = EntityState.Modified;
            //    }
            //}
        }

        #endregion 

        private IDbSet<T> GetSet()
        {
            return efContext.DbContext.Set<T>();
        }
    }
}
