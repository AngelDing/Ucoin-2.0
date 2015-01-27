using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastProperty
    {
        private static readonly ConcurrentDictionary<TypeMemberKey, PropertyInfo> _propertyInfoCache =
                new ConcurrentDictionary<TypeMemberKey, PropertyInfo>();

        private static readonly ConcurrentDictionary<TypeMemberKey, FastProperty> _propertyCache =
            new ConcurrentDictionary<TypeMemberKey, FastProperty>();

        private Lazy<Func<object, object>> _getter;
        private Lazy<Action<object, object>> _setter;

        private FastProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            Property = property;
            _getter = new Lazy<Func<object, object>>(() => GetGetMethod(property));
            _setter = new Lazy<Action<object, object>>(() => GetSetMethod(property));
        }

        public PropertyInfo Property { get; private set; }

        public object Get(object instance)
        {
            return _getter.Value(instance);
        }

        public void Set(object instance, object value)
        {
            _setter.Value(instance, value);
        }

        private static Action<object, object> GetSetMethod(PropertyInfo property)
        {
            MethodInfo method = property.GetSetMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No setter available on " + property.Name); };
            }
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");

            // value as T is slightly faster than (T)value, so if it's not a value type, use that
            UnaryExpression instanceCast = property.DeclaringType.IsValueType
                ? Expression.Convert(instance, property.DeclaringType)
                : Expression.TypeAs(instance, property.DeclaringType);

            UnaryExpression valueCast = property.PropertyType.IsValueType
                ? Expression.Convert(value, property.PropertyType)
                : Expression.TypeAs(value, property.PropertyType);

            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, valueCast)
                : Expression.Call(instanceCast, method, valueCast);

            return Expression.Lambda<Action<object, object>>(call, instance, value).Compile();
        }

        private static Func<object, object> GetGetMethod(PropertyInfo property)
        {
            MethodInfo method = property.GetGetMethod(true);
            if (method == null)
            {
                return x => { throw new InvalidOperationException("No getter available on " + property.Name); };
            }
            ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
            UnaryExpression instanceCast = property.DeclaringType.IsValueType
                ? Expression.Convert(instance, property.DeclaringType)
                : Expression.TypeAs(instance, property.DeclaringType);
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method)
                : Expression.Call(instanceCast, method);
            return Expression.Lambda<Func<object, object>>(Expression.TypeAs(call, typeof(object)), instance).Compile();
        }

        internal static PropertyInfo GetProperty(Type type, string propertyName)
        {
            return _propertyInfoCache.GetOrAdd(new TypeMemberKey(type, propertyName), key => FindProperty(key.Type, key.MemberName));
        }

        private static PropertyInfo FindProperty(Type type, string name)
        {
            PropertyInfo property = type.GetProperty(name,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (property == null)
            {
                Type baseType = type.BaseType;
                if (baseType != null)
                {
                    property = GetProperty(baseType, name);
                }
            }
            return property;
        }

        public static FastProperty Get(Type type, string name)
        {
            return _propertyCache.GetOrAdd(new TypeMemberKey(type, name), key => CreateProperty(key.Type, key.MemberName));
        }

        private static FastProperty CreateProperty(Type type, string name)
        {
            PropertyInfo property = GetProperty(type, name);
            return property == null ? null : new FastProperty(property);
        }
    }
}