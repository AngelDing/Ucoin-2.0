using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastField<T, TField>
    {
        private static readonly ConcurrentDictionary<string, FastField<T, TField>> _fieldCache =
            new ConcurrentDictionary<string, FastField<T, TField>>();

        private readonly Lazy<Func<T, TField>> _getter;
        private readonly Lazy<Action<T, TField>> _setter;

        private FastField(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            Field = field;
            _getter = new Lazy<Func<T, TField>>(() => GetGetMethod(field));
            _setter = new Lazy<Action<T, TField>>(() => GetSetMethod(field));
        }

        public FieldInfo Field { get; private set; }

        public TField Get(T instance)
        {
            return _getter.Value(instance);
        }

        public void Set(T instance, TField value)
        {
            _setter.Value(instance, value);
        }

        private static Action<T, TField> GetSetMethod(FieldInfo field)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression value = Expression.Parameter(typeof(TField), "value");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));
            Expression valueCast = field.FieldType != typeof(TField)
                ? field.FieldType.IsValueType
                    ? Expression.Convert(value, field.FieldType)
                    : Expression.TypeAs(value, field.FieldType)
                : (Expression)value;

            MemberExpression fieldExpression = Expression.Field(field.IsStatic ? null : instanceCast, field);
            BinaryExpression call = Expression.Assign(fieldExpression, valueCast);

            return Expression.Lambda<Action<T, TField>>(call, instance, value).Compile();
        }

        private static Func<T, TField> GetGetMethod(FieldInfo field)
        {
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));

            MemberExpression call = Expression.Field(field.IsStatic ? null : instanceCast, field);
            Expression callCast = field.FieldType != typeof(TField)
                ? typeof(TField).IsValueType
                    ? Expression.Convert(call, typeof(TField))
                    : Expression.TypeAs(call, typeof(TField))
                : (Expression)call;
            return Expression.Lambda<Func<T, TField>>(callCast, instance).Compile();
        }

        public static FastField<T, TField> Get(string name)
        {
            return _fieldCache.GetOrAdd(name, CreateField);
        }

        private static FastField<T, TField> CreateField(string name)
        {
            FieldInfo info = FastField.GetField(typeof(T), name);
            return info == null ? null : new FastField<T, TField>(info);
        }
    }
}