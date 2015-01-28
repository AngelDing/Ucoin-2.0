using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class ExpressionExtensions
    {
        public static FieldInfo GetMemberField<T, TMember>(this Expression<Func<T, TMember>> expression)
        {
            return expression.GetMemberExpression().Member as FieldInfo;
        }

        public static PropertyInfo GetMemberProperty<T, TMember>(this Expression<Func<T, TMember>> expression)
        {
            return expression.GetMemberExpression().Member as PropertyInfo;
        }

        public static MemberExpression GetMemberExpression<T>(this Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            return GetMemberExpression(expression.Body);
        }

        public static MemberExpression GetMemberExpression<T1, T2>(this Expression<Action<T1, T2>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            return GetMemberExpression(expression.Body);
        }

        public static MemberExpression GetMemberExpression<T, TMember>(this Expression<Func<T, TMember>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            return GetMemberExpression(expression.Body);
        }

        private static MemberExpression GetMemberExpression(Expression body)
        {
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            MemberExpression memberExpression = null;
            if (body.NodeType == ExpressionType.Convert)
            {
                var unaryExpression = (UnaryExpression) body;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else if (body.NodeType == ExpressionType.MemberAccess)
                memberExpression = body as MemberExpression;

            if (memberExpression == null)
                throw new ArgumentException("Expression is not a member access");

            return memberExpression;
        }
    }
}