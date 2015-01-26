using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
namespace Ucoin.Framework.Entities
{
    [Serializable]
    public abstract class BaseEntity : IEntity, IPartialUpdateEntity
    {
        private Dictionary<string, object> updateList = new Dictionary<string, object>();
        public Dictionary<string, object> NeedUpdateList
        {
            get
            {
                return updateList;
            }
        }

        /// <summary>
        /// 是否局部更新
        /// </summary>
        public virtual bool IsPartialUpdate { get; set; }

        /// <summary>
        /// 用於實體的指定字段的更新
        /// </summary>
        /// <typeparam name="T">實體類型</typeparam>
        /// <param name="express">表達式： p => p.XXX</param>
        /// <param name="val">要更新的值</param>
        public virtual void SetUpdate<T>(Expression<Func<T>> express, object val)
        {
            if (express == null || express.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("'" + express + "': 不是有效的表達式！");
            }
            MemberExpression body = (MemberExpression)express.Body;
            var propStr = GetUpdateKey(express);

            updateList.Add(propStr, val);

            var type = this.GetType();
            var pd = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>()
                .Where(i => i.DisplayName == propStr).FirstOrDefault();
            if (pd != null)
            {
                pd.SetValue(this, val);
            }
        }

        public abstract string GetUpdateKey(LambdaExpression expression);
    }

    [Serializable]
    public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
    {
        [CompareKey]
        public TKey Id { get; set; }

        public override bool Equals(object entity)
        {
            return entity != null
               && entity is BaseEntity<TKey>
               && this == (BaseEntity<TKey>)entity;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
            {
                return true;
            }

            if ((object)entity1 == null || (object)entity2 == null)
            {
                return false;
            }

            if (entity1.Id.ToString() == entity2.Id.ToString())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(BaseEntity<TKey> entity1, BaseEntity<TKey> entity2)
        {
            return (!(entity1 == entity2));
        }
    }
}
