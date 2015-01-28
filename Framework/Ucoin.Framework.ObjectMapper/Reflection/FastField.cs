using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastField
    {
        private static readonly ConcurrentDictionary<TypeMemberKey, FieldInfo> _fieldInfoCache =
            new ConcurrentDictionary<TypeMemberKey, FieldInfo>();

        private static readonly ConcurrentDictionary<TypeMemberKey, FastField> _fieldCache =
            new ConcurrentDictionary<TypeMemberKey, FastField>();

        private Lazy<Func<object, object>> _getter;
        private Lazy<Action<object, object>> _setter;

        private FastField(FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            Field = field;
            _getter = new Lazy<Func<object, object>>(() => GetGetMethod(field));
            _setter = new Lazy<Action<object, object>>(() => GetSetMethod(field));
        }

        public FieldInfo Field { get; private set; }

        public object Get(object instance)
        {
            return _getter.Value(instance);
        }

        public void Set(object instance, object value)
        {
            _setter.Value(instance, value);
        }

        private static Action<object, object> GetSetMethod(FieldInfo field)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            UnaryExpression instanceCast = field.DeclaringType.IsValueType
                ? Expression.Convert(instance, field.DeclaringType)
                : Expression.TypeAs(instance, field.DeclaringType);

            UnaryExpression valueCast = field.FieldType.IsValueType
                ? Expression.Convert(value, field.FieldType)
                : Expression.TypeAs(value, field.FieldType);

            MemberExpression fieldExpression = Expression.Field(field.IsStatic ? null : instanceCast, field);
            BinaryExpression call = Expression.Assign(fieldExpression, valueCast);

            return Expression.Lambda<Action<object, object>>(call, instance, value).Compile();
        }

        private static Func<object, object> GetGetMethod(FieldInfo field)
        {
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression instanceCast = field.DeclaringType.IsValueType
                ? Expression.Convert(instance, field.DeclaringType)
                : Expression.TypeAs(instance, field.DeclaringType);

            MemberExpression call = Expression.Field(field.IsStatic ? null : instanceCast, field);
            return Expression.Lambda<Func<object, object>>(Expression.TypeAs(call, typeof(object)), instance).Compile();
        }

        internal static FieldInfo GetField(Type type, string fieldName)
        {
            return _fieldInfoCache.GetOrAdd(new TypeMemberKey(type, fieldName), key => FindField(key.Type, key.MemberName));
        }

        private static FieldInfo FindField(Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
            {
                Type baseType = type.BaseType;
                if (baseType != null)
                {
                    field = GetField(baseType, fieldName);
                }
            }
            return field;
        }

        public static FastField Get(Type type, string name)
        {
            return _fieldCache.GetOrAdd(new TypeMemberKey(type, name), key => CreateField(key.Type, key.MemberName));
        }

        private static FastField CreateField(Type type, string name)
        {
            FieldInfo field = GetField(type, name);
            return field == null ? null : new FastField(field);
        }
    }
}