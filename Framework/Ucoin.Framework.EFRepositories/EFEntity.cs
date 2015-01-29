using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Ucoin.Framework.Entities
{
    [Serializable]
    public class EFEntity<Tkey> : BaseEntity<Tkey>, IValidatableObject
    {
        public EFEntity()
        {
            IsPartialUpdate = false;
        }

        /// <summary>
        /// 實體所處的操作狀態
        /// </summary>
        [NotMapped]
        [CompareIgnore]
        public override ObjectStateType ObjectState { get; set; }

        /// <summary>
        /// 是否局部更新
        /// </summary>
        [NotMapped]
        [CompareIgnore]
        public override bool IsPartialUpdate { get; set; }

        public override void SetUpdate<T>(Expression<Func<T>> express, object val)
        {
            base.SetUpdate<T>(express, val);
            this.IsPartialUpdate = true;
            this.ObjectState = ObjectStateType.Modified;
        }

        /// <summary>
        /// 獲取傳入表達式的所要更新的屬性
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override string GetUpdateKey(LambdaExpression expression)
        {
            var keys = new List<string>();
            var body = expression.Body;
            while (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                var baseType = memberExpression.Type.BaseType;
                if (baseType == this.GetType().BaseType)
                {
                    break;
                }
                keys.Add(memberExpression.Member.Name);

                var insideExpress = memberExpression.Expression;
                if (insideExpress != null && insideExpress.NodeType == ExpressionType.MemberAccess)
                {
                    body = insideExpress;
                }
                else
                {
                    break;
                }
            }

            keys.Reverse();
            return string.Join(".", keys.ToArray());
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}
