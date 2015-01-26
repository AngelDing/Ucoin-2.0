using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Ucoin.Framework.Entities
{
    [Serializable]
    public class EFEntity<Tkey> : BaseEntity<Tkey>, IObjectWithState, IValidatableObject
    {
        public EFEntity()
        {
            IsPartialUpdate = false;
        }

        /// <summary>
        /// 實體所處的操作狀態
        /// </summary>
        [CompareIgnore]
        [NotMapped]
        public ObjectStateType State { get; set; }

        /// <summary>
        /// 是否局部更新
        /// </summary>
        [CompareIgnore]
        [NotMapped]
        public override bool IsPartialUpdate { get; set; }

        public override void SetUpdate<T>(Expression<Func<T>> express, object val)
        {
            base.SetUpdate<T>(express, val);
            this.IsPartialUpdate = true;
            this.State = ObjectStateType.Modified;
        }

        /// <summary>
        /// 獲取傳入表達式的所要更新的屬性
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override string GetUpdateKey(LambdaExpression expression)
        {
            var prop = string.Empty;
            var body = expression.Body;
           
            if (body != null)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                prop = memberExpression.Member.Name;
            }

            return prop;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
