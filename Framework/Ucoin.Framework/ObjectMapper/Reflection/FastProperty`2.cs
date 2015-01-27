using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastProperty<T, TProperty>
    {
        #region Fields

        private static readonly ConcurrentDictionary<string, FastProperty<T, TProperty>> _propertyCache =
            new ConcurrentDictionary<string, FastProperty<T, TProperty>>();

        private Lazy<Func<T, TProperty>> _getter;
        private Lazy<Action<T, TProperty>> _setter;

        #endregion

        #region Constructors

        private FastProperty(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            Property = property;
            _getter = new Lazy<Func<T, TProperty>>(() => GetGetMethod(property));
            _setter = new Lazy<Action<T, TProperty>>(() => GetSetMethod(property));
        }

        #endregion

        public PropertyInfo Property { get; private set; }

        #region Methods

        public TProperty Get(T instance)
        {
            return _getter.Value(instance);
        }

        public void Set(T instance, TProperty value)
        {
            _setter.Value(instance, value);
        }

        public static FastProperty<T, TProperty> Get(string name)
        {
            return _propertyCache.GetOrAdd(name, CreateProperty);
        }

        private static Action<T, TProperty> GetSetMethod(PropertyInfo property)
        {
            MethodInfo method = property.GetSetMethod(true);
            if (method == null)
            {
                return (x, i) => { throw new InvalidOperationException("No setter available on " + property.Name); };
            }
            ParameterExpression instance = Expression.Parameter(typeof(T), "instance");
            ParameterExpression value = Expression.Parameter(typeof(TProperty), "value");
            Expression instanceCast = typeof(T).IsValueType || typeof(T).IsSealed
                ? (Expression)instance
                : Expression.TypeAs(instance, typeof(T));
            Expression valueCast = property.PropertyType != typeof(TProperty)
                ? property.PropertyType.IsValueType
                    ? Expression.Convert(value, property.PropertyType)
                    : Expression.TypeAs(value, property.PropertyType)
                : (Expression)value;
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, valueCast)
                : Expression.Call(instanceCast, method, valueCast);

            return Expression.Lambda<Action<T, TProperty>>(call, instance, value).Compile();
        }

        private static Func<T, TProperty> GetGetMethod(PropertyInfo property)
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
            Expression callCast = property.PropertyType != typeof(TProperty)
                ? typeof(TProperty).IsValueType
                    ? Expression.Convert(call, typeof(TProperty))
                    : Expression.TypeAs(call, typeof(TProperty))
                : (Expression)call;
            return Expression.Lambda<Func<T, TProperty>>(callCast, instance).Compile();
        }

        private static FastProperty<T, TProperty> CreateProperty(string name)
        {
            PropertyInfo info = FastProperty.GetProperty(typeof(T), name);
            return info == null ? null : new FastProperty<T, TProperty>(info);
        }

        #endregion
    }
}