using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastInvoker<T> : FastInvokerBase
    {
        #region Fields

        [ThreadStatic] 
        private static FastInvoker<T> _current;

        private readonly ConcurrentDictionary<int, Func<T, object>> _noArgs = new ConcurrentDictionary<int, Func<T, object>>();

        private readonly ConcurrentDictionary<int, Func<T, object[], object>> _withArgs =
            new ConcurrentDictionary<int, Func<T, object[], object>>();

        #endregion

        #region Constructors

        private FastInvoker()
            : base(typeof (T))
        {
        }

        #endregion

        #region Properties

        internal static FastInvoker<T> Current
        {
            get { return _current ?? (_current = new FastInvoker<T>()); }
        }

        #endregion

        #region Invoke

        public object Invoke(T target, string methodName)
        {
            return GetInvoker(methodName)(target);
        }

        public object Invoke(T target, string methodName, params object[] args)
        {
            return GetInvoker(methodName, args)(target, args);
        }

        public object Invoke(T target, Type[] genericTypes, string methodName)
        {
            return GetInvoker(genericTypes, methodName)(target);
        }

        public object Invoke(T target, Type[] genericTypes, string methodName, params object[] args)
        {
            return GetInvoker(genericTypes, methodName, args)(target, args);
        }

        public object Invoke(T target, Expression<Action<T>> expression)
        {
            return GetInvoker(ExtractMethod(expression))(target);
        }

        public object Invoke(T target, Expression<Action<T>> expression, params object[] args)
        {
            return GetInvoker(ExtractMethod(expression), args)(target, args);
        }

        public object Invoke(T target, Type[] genericTypes, Expression<Action<T>> expression)
        {
            return GetInvoker(genericTypes, ExtractMethod(expression))(target);
        }

        public object Invoke(T target, Type[] genericTypes, Expression<Action<T>> expression, params object[] args)
        {
            return GetInvoker(genericTypes, ExtractMethod(expression), args)(target, args);
        }

        private MethodInfo ExtractMethod(Expression<Action<T>> expression)
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
            return 97*methodName.GetHashCode();
        }

        private int GetHashCodeFeed(MethodInfo method)
        {
            return 61*method.GetHashCode();
        }

        private Func<T, object> GetInvoker(MethodInfo method)
        {
            return GetInvoker(GetHashCodeFeed(method), () => method);
        }

        private Func<T, object[], object> GetInvoker(MethodInfo method, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, object> invoker = GetInvoker(method);
                return (x, y) => invoker(x);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), args),
                () => method.IsGenericMethod
                    ? method.GetGenericMethodDefinition().ToSpecializedMethod(args)
                    : method, args);
        }

        private Func<T, object> GetInvoker(Type[] genericTypes, MethodInfo method)
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

        private Func<T, object[], object> GetInvoker(Type[] genericTypes, MethodInfo method, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, object> invoker = GetInvoker(genericTypes, method);
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

        private Func<T, object> GetInvoker(Type[] genericTypes, string methodName)
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

        private Func<T, object[], object> GetInvoker(Type[] genericTypes, string methodName, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, object> invoker = GetInvoker(genericTypes, methodName);
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

        private Func<T, object> GetInvoker(string methodName)
        {
            return GetInvoker(GetHashCodeFeed(methodName),
                () => GetMethods(methodName).MatchingArguments().FirstOrDefault());
        }

        private Func<T, object[], object> GetInvoker(string methodName, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, object> invoker = GetInvoker(methodName);
                return (x, y) => invoker(x);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(methodName), args),
                () => GetMethods(methodName)
                    .MatchingArguments(args)
                    .Select(x => x.ToSpecializedMethod(args))
                    .FirstOrDefault(), args);
        }

        private Func<T, object> GetInvoker(int key, Func<MethodInfo> getMethodInfo)
        {
            return _noArgs.GetOrAdd(key, new Func<int, Func<T, object>>(k => CreateInvoker(getMethodInfo)));
        }

        private Func<T, object> CreateInvoker(Func<MethodInfo> getMethodInfo)
        {
            MethodInfo method = getMethodInfo();
            if (method == null) throw new FastReflectionException(ObjectType, "No available method.");
            ParameterExpression instanceParameter = Expression.Parameter(typeof(T), "target");
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method)
                : Expression.Call(instanceParameter, method);
            if (method.ReturnType != typeof (void))
            {
                return
                    Expression.Lambda<Func<T, object>>(Expression.TypeAs(call, typeof(object)),
                        instanceParameter).Compile();
            }
            Action<T> action = Expression.Lambda<Action<T>>(call, instanceParameter).Compile();
            return target =>
            {
                action(target);
                return null;
            };
        }

        private Func<T, object[], object> GetInvoker(int key, Func<MethodInfo> getMethodInfo, object[] args)
        {
            return _withArgs.GetOrAdd(key, k => CreateInvoker(getMethodInfo, args));
        }

        private Func<T, object[], object> CreateInvoker(Func<MethodInfo> getMethodInfo, object[] args)
        {
            MethodInfo method = getMethodInfo();
            if (method == null) throw new FastReflectionException(ObjectType, "No available method.");
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression[] parameters = method.GetParameters().ToArrayIndexParameters(argsParameter).ToArray();
            ParameterExpression instanceParameter = Expression.Parameter(typeof(T), "target");
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, parameters)
                : Expression.Call(instanceParameter, method, parameters);
            if (method.ReturnType != typeof (void))
            {
                return
                    Expression.Lambda<Func<T, object[], object>>(Expression.TypeAs(call, typeof (object)),
                        instanceParameter, argsParameter).Compile();
            }
            Action<T, object[]> action =
                Expression.Lambda<Action<T, object[]>>(call, instanceParameter, argsParameter).Compile();
            return (target, arguments) =>
            {
                action(target, arguments);
                return null;
            };
        }

        #endregion
    }
}