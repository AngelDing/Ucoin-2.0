using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastField<T>
    {
        private static readonly ConcurrentDictionary<string, FastField<T>> _fieldCache =
            new ConcurrentDictionary<string, FastField<T>>();

        private readonly Lazy<Func<T, object>> _getter;
        private readonly Lazy<Action<T, object>> _setter;

        private FastField(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            Field = field;
            _getter = new Lazy<Func<T, object>>(() => GetGetMethod(field));
            _setter = new Lazy<Action<T, object>>(() => GetSetMethod(field));
        }

        public FieldInfo Field { get; private set; }

        public object Get(T instance)
        {
            return _getter.Value(instance);
        }

        public void Set(T instance, object value)
        {
            _setter.Value(instance, value);
        }

        private static Action<T, object> GetSetMethod(FieldInfo field)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));
            UnaryExpression valueCast = field.FieldType.IsValueType
                ? Expression.Convert(value, field.FieldType)
                : Expression.TypeAs(value, field.FieldType);

            MemberExpression fieldExpression = Expression.Field(field.IsStatic ? null : instanceCast, field);
            BinaryExpression call = Expression.Assign(fieldExpression, valueCast);

            return Expression.Lambda<Action<T, object>>(call, instance, value).Compile();
        }

        private static Func<T, object> GetGetMethod(FieldInfo field)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));
            MemberExpression call = Expression.Field(field.IsStatic ? null : instanceCast, field);
            UnaryExpression callCast = Expression.TypeAs(call, typeof(object));
            return Expression.Lambda<Func<T, object>>(callCast, instance).Compile();
        }

        public static FastField<T> Get(string name)
        {
            return _fieldCache.GetOrAdd(name, CreateField);
        }

        private static FastField<T> CreateField(string name)
        {
            FieldInfo info = FastField.GetField(typeof(T), name);
            return info == null ? null : new FastField<T>(info);
        }
    }
}