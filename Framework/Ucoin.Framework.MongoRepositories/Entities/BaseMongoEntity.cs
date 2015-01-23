using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ucoin.Framework.Entities
{
    [Serializable]
    public abstract class BaseMongoEntity : IEntity
    {
        private Dictionary<string, object> updateList = new Dictionary<string, object>();
        internal Dictionary<string, object> NeedUpdateList
        {
            get
            {
                return updateList;
            }
        }

        public void SetUpdate<T>(Expression<Func<T>> express, object val)
        {
            if (express == null || express.Body.NodeType != ExpressionType.MemberAccess)
            {
                throw new ArgumentException("'" + express + "': 不是有效的表達式！");
            }
            MemberExpression body = (MemberExpression)express.Body;
            var propStr = GetUpdateKey(express);

            updateList.Add(propStr, val);
        }

        private string GetUpdateKey(LambdaExpression expression)
        {
            var keys = new List<string>();
            var body = expression.Body;
            while (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression memberExpression = (MemberExpression)body;
                var baseType = memberExpression.Type.BaseType;
                if (baseType == typeof(IntKeyMongoEntity)
                    || baseType == typeof(StringKeyMongoEntity)
                    || baseType == typeof(LongKeyMongoEntity))
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
    }
}
