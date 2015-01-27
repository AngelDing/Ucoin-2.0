using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastInvoker : FastInvokerBase
    {
        #region Fields

        private static readonly ConcurrentDictionary<Type, FastInvoker> _cache =
            new ConcurrentDictionary<Type, FastInvoker>();

        private readonly ConcurrentDictionary<int, Func<object, object>> _noArgs =
            new ConcurrentDictionary<int, Func<object, object>>();

        private readonly ConcurrentDictionary<int, Func<object, object[], object>> _withArgs =
            new ConcurrentDictionary<int, Func<object, object[], object>>();

        #endregion

        private FastInvoker(Type type)
            : base(type)
        {
        }

        #region Invoke

        public object Invoke(object target, string methodName)
        {
            return GetInvoker(methodName)(target);
        }

        public object Invoke(object target, string methodName, params object[] args)
        {
            return GetInvoker(methodName, args)(target, args);
        }

        public object Invoke(object target, Type[] genericTypes, string methodName)
        {
            return GetInvoker(genericTypes, methodName)(target);
        }

        public object Invoke(object target, Type[] genericTypes, string methodName, params object[] args)
        {
            return GetInvoker(genericTypes, methodName, args)(target, args);
        }

        public object Invoke(object target, LambdaExpression expression)
        {
            return GetInvoker(ExtractMethod(expression))(target);
        }

        public object Invoke(object target, LambdaExpression expression, params object[] args)
        {
            return GetInvoker(ExtractMethod(expression), args)(target, args);
        }

        public object Invoke(object target, Type[] genericTypes, LambdaExpression expression)
        {
            return GetInvoker(genericTypes, ExtractMethod(expression))(target);
        }

        public object Invoke(object target, Type[] genericTypes, LambdaExpression expression, params object[] args)
        {
            return GetInvoker(genericTypes, ExtractMethod(expression), args)(target, args);
        }

        private MethodInfo ExtractMethod(LambdaExpression expression)
        {
            var call = expression.Body as MethodCallExpression;
            if (call == null)
                throw new ArgumentException("Only method call expressions are supported.", "expression");
            return call.Method;
        }

        #endregion

        #region GetInvoker

        private int GetHashCodeFeed(string methodName)
        {
            return 97 * ((97 * methodName.GetHashCode()) ^ ObjectType.GetHashCode());
        }

        private int GetHashCodeFeed(MethodInfo method)
        {
            return 61 * ((61 * method.GetHashCode()) ^ ObjectType.GetHashCode());
        }

        private Func<object, object> GetInvoker(MethodInfo method)
        {
            return GetInvoker(GetHashCodeFeed(method), () => method);
        }

        private Func<object, object[], object> GetInvoker(MethodInfo method, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<object, object> invoker = GetInvoker(method);
                return (x, y) => invoker(x);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), args),
                () => method.IsGenericMethod
                    ? method.GetGenericMethodDefinition().ToSpecializedMethod(args)
                    : method, args);
        }

        private Func<object, object> GetInvoker(Type[] genericTypes, MethodInfo method)
        {
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(method);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), genericTypes),
                () =>
                    method.IsGenericMethod
                        ? GetGenericMethodFromTypes(method.GetGenericMethodDefinition(), genericTypes)
                        : method);
        }

        private Func<object, object[], object> GetInvoker(Type[] genericTypes, MethodInfo method, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<object, object> invoker = GetInvoker(genericTypes, method);
                return (x, y) => invoker(x);
            }
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(method, args);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), genericTypes, args),
                () =>
                    method.IsGenericMethod
                        ? method.GetGenericMethodDefinition().ToSpecializedMethod(genericTypes, args)
                        : method.ToSpecializedMethod(genericTypes, args), args);
        }

        private Func<object, object> GetInvoker(string methodName)
        {
            return GetInvoker(GetHashCodeFeed(methodName),
                () => GetMethods(methodName).MatchingArguments().FirstOrDefault());
        }

        private Func<object, object[], object> GetInvoker(string methodName, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<object, object> invoker = GetInvoker(methodName);
                return (x, y) => invoker(x);
            }
            return
                GetInvoker(
                    GetArgumentHashCode(GetHashCodeFeed(methodName), args),
                    () => GetMethods(methodName)
                        .MatchingArguments(args)
                        .Select(x => x.ToSpecializedMethod(args))
                        .FirstOrDefault(), args);
        }

        private Func<object, object> GetInvoker(Type[] genericTypes, string methodName)
        {
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(methodName);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(methodName), genericTypes),
                () =>
                    GetMethods(methodName)
                        .MatchingArguments()
                        .Select(x => x.ToSpecializedMethod(genericTypes, new object[0]))
                        .FirstOrDefault());
        }

        private Func<object, object[], object> GetInvoker(Type[] genericTypes, string methodName, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<object, object> invoker = GetInvoker(genericTypes, methodName);
                return (x, y) => invoker(x);
            }
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(methodName, args);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(methodName), genericTypes, args),
                () => GetMethods(methodName)
                    .MatchingArguments(genericTypes, args)
                    .Select(x => x.ToSpecializedMethod(genericTypes, args))
                    .FirstOrDefault(), args);
        }

        private Func<object, object> GetInvoker(int key, Func<MethodInfo> getMethodInfo)
        {
            return _noArgs.GetOrAdd(key, new Func<int, Func<object, object>>(k => CreateInvoker(getMethodInfo)));
        }

        private Func<object, object[], object> GetInvoker(int key, Func<MethodInfo> getMethodInfo, object[] args)
        {
            return _withArgs.GetOrAdd(key, k => CreateInvoker(getMethodInfo, args));
        }

        private Func<object, object> CreateInvoker(Func<MethodInfo> getMethodInfo)
        {
            MethodInfo method = getMethodInfo();
            if (method == null) throw new FastReflectionException(ObjectType, "No available method.");
            ParameterExpression instance = Expression.Parameter(typeof(object), "target");
            UnaryExpression instanceCast = method.DeclaringType.IsValueType
                ? Expression.Convert(instance, method.DeclaringType)
                : Expression.TypeAs(instance, method.DeclaringType);
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method)
                : Expression.Call(instanceCast, method);
            if (method.ReturnType != typeof(void))
            {
                return
                    Expression.Lambda<Func<object, object>>(Expression.TypeAs(call, typeof(object)),
                        instance).Compile();
            }
            Action<object> action = Expression.Lambda<Action<object>>(call, instance).Compile();
            return target =>
            {
                action(target);
                return null;
            };
        }

        private Func<object, object[], object> CreateInvoker(Func<MethodInfo> getMethodInfo, object[] args)
        {
            MethodInfo method = getMethodInfo();
            if (method == null) throw new FastReflectionException(ObjectType, "No available method.");
            ParameterExpression instance = Expression.Parameter(typeof(object), "target");
            UnaryExpression instanceCast = method.DeclaringType.IsValueType
                ? Expression.Convert(instance, method.DeclaringType)
                : Expression.TypeAs(instance, method.DeclaringType);
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression[] parameters = method.GetParameters().ToArrayIndexParameters(argsParameter).ToArray();
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, parameters)
                : Expression.Call(instanceCast, method, parameters);
            if (method.ReturnType != typeof(void))
            {
                return
                    Expression.Lambda<Func<object, object[], object>>(Expression.TypeAs(call, typeof(object)),
                        instance, argsParameter).Compile();
            }
            Action<object, object[]> action =
                Expression.Lambda<Action<object, object[]>>(call, instance, argsParameter).Compile();
            return (target, arguments) =>
            {
                action(target, arguments);
                return null;
            };
        }

        #endregion

        public static FastInvoker Get(Type type)
        {
            return _cache.GetOrAdd(type, CreateInvoker);
        }

        private static FastInvoker CreateInvoker(Type type)
        {
            return new FastInvoker(type);
        }
    }
}