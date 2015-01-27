using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastProperty<T>
    {
        private static readonly ConcurrentDictionary<string, FastProperty<T>> _propertyCache =
            new ConcurrentDictionary<string, FastProperty<T>>();

        private readonly Lazy<Func<T, object>> _getter;
        private readonly Lazy<Action<T, object>> _setter;

        private FastProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            Property = property;
            _getter = new Lazy<Func<T, object>>(() => GetGetMethod(property));
            _setter = new Lazy<Action<T, object>>(() => GetSetMethod(property));
        }

        public PropertyInfo Property { get; private set; }

        public object Get(T instance)
        {
            return _getter.Value(instance);
        }

        public void Set(T instance, object value)
        {
            _setter.Value(instance, value);
        }

        private static Action<T, object> GetSetMethod(PropertyInfo property)
        {
            MethodInfo method = property.GetSetMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No setter available on " + property.Name); };
            }
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression value = Expression.Parameter(typeof(object), "value");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));
            UnaryExpression valueCast = property.PropertyType.IsValueType
                ? Expression.Convert(value, property.PropertyType)
                : Expression.TypeAs(value, property.PropertyType);

            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, valueCast)
                : Expression.Call(instanceCast, method, valueCast);

            return Expression.Lambda<Action<T, object>>(call, instance, value).Compile();
        }

        private static Func<T, object> GetGetMethod(PropertyInfo property)
        {
            MethodInfo method = property.GetGetMethod(true);
            if (method == null)
            {
                return x => { throw new InvalidOperationException("No getter available on " + property.Name); };
            }
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method)
                : Expression.Call(instanceCast, method);
            return Expression.Lambda<Func<T, object>>(Expression.TypeAs(call, typeof(object)), instance).Compile();
        }

        public static FastProperty<T> Get(string name)
        {
            return _propertyCache.GetOrAdd(name, CreateProperty);
        }

        private static FastProperty<T> CreateProperty(string name)
        {
            PropertyInfo info = FastProperty.GetProperty(typeof(T), name);
            return info == null ? null : new FastProperty<T>(info);
        }
    }
}