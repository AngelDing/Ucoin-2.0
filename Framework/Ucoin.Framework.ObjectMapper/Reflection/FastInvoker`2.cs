using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastInvoker<T, TResult> : FastInvokerBase
    {
        #region Fields

        [ThreadStatic] 
        private static FastInvoker<T, TResult> _current;

        private readonly ConcurrentDictionary<int, Func<T, TResult>> _noArgs =
            new ConcurrentDictionary<int, Func<T, TResult>>();

        private readonly ConcurrentDictionary<int, Func<T, object[], TResult>> _withArgs =
            new ConcurrentDictionary<int, Func<T, object[], TResult>>();

        #endregion

        #region Constructors

        private FastInvoker()
            : base(typeof (T))
        {
        }

        #endregion

        #region Properties

        internal static FastInvoker<T, TResult> Current
        {
            get { return _current ?? (_current = new FastInvoker<T, TResult>()); }
        }

        #endregion

        #region Invoke

        public TResult Invoke(T target, string methodName)
        {
            return GetInvoker(methodName)(target);
        }

        public TResult Invoke(T target, string methodName, params object[] args)
        {
            return GetInvoker(methodName, args)(target, args);
        }

        public TResult Invoke(T target, Type[] genericTypes, string methodName)
        {
            return GetInvoker(genericTypes, methodName)(target);
        }

        public TResult Invoke(T target, Type[] genericTypes, string methodName, object[] args)
        {
            return GetInvoker(genericTypes, methodName, args)(target, args);
        }

        public TResult Invoke(T target, Expression<Func<T, TResult>> expression)
        {
            return GetInvoker(ExtractMethod(expression))(target);
        }

        public TResult Invoke(T target, Expression<Func<T, TResult>> expression, params object[] args)
        {
            return GetInvoker(ExtractMethod(expression), args)(target, args);
        }

        public TResult Invoke(T target, Type[] genericTypes, Expression<Func<T, TResult>> expression)
        {
            return GetInvoker(genericTypes, ExtractMethod(expression))(target);
        }

        public TResult Invoke(T target, Type[] genericTypes, Expression<Func<T, TResult>> expression, object[] args)
        {
            return GetInvoker(genericTypes, ExtractMethod(expression), args)(target, args);
        }

        private MethodInfo ExtractMethod(Expression<Func<T, TResult>> expression)
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

        private Func<T, TResult> GetInvoker(MethodInfo method)
        {
            return GetInvoker(GetHashCodeFeed(method), () => method);
        }

        private Func<T, object[], TResult> GetInvoker(MethodInfo method, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, TResult> invoker = GetInvoker(method);
                return (x, y) => invoker(x);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), args),
                () => method.IsGenericMethod
                    ? method.GetGenericMethodDefinition().ToSpecializedMethod(args)
                    : method, args);
        }

        private Func<T, TResult> GetInvoker(Type[] genericTypes, MethodInfo method)
        {
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(method);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), genericTypes),
                () => method.IsGenericMethod
                    ? GetGenericMethodFromTypes(method.GetGenericMethodDefinition(), genericTypes)
                    : method);
        }

        private Func<T, object[], TResult> GetInvoker(Type[] genericTypes, MethodInfo method, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, TResult> invoker = GetInvoker(genericTypes, method);
                return (x, y) => invoker(x);
            }
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(method, args);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(method), genericTypes, args),
                () => method.IsGenericMethod
                    ? method.GetGenericMethodDefinition().ToSpecializedMethod(genericTypes, args)
                    : method.ToSpecializedMethod(genericTypes, args), args);
        }

        private Func<T, TResult> GetInvoker(Type[] genericTypes, string methodName)
        {
            if (genericTypes == null || genericTypes.Length == 0)
            {
                return GetInvoker(methodName);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(methodName), genericTypes),
                () => GetMethods(methodName).MatchingArguments()
                    .Select(x => x.ToSpecializedMethod(genericTypes, new object[0]))
                    .First(x => x.ReturnType == typeof (TResult)));
        }

        private Func<T, object[], TResult> GetInvoker(Type[] genericTypes, string methodName, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, TResult> invoker = GetInvoker(genericTypes, methodName);
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
                    .First(x => x.ReturnType == typeof (TResult)), args);
        }

        private Func<T, TResult> GetInvoker(string methodName)
        {
            return GetInvoker(GetHashCodeFeed(methodName),
                () => GetMethods(methodName)
                    .MatchingArguments()
                    .FirstOrDefault(x => x.ReturnType == typeof (TResult)));
        }

        private Func<T, object[], TResult> GetInvoker(string methodName, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Func<T, TResult> invoker = GetInvoker(methodName);
                return (x, y) => invoker(x);
            }
            return GetInvoker(GetArgumentHashCode(GetHashCodeFeed(methodName), args),
                () => GetMethods(methodName)
                    .MatchingArguments(args)
                    .FirstOrDefault(x => x.ReturnType == typeof (TResult))
                    .ToSpecializedMethod(args), args);
        }

        private Func<T, TResult> GetInvoker(int key, Func<MethodInfo> getMethodInfo)
        {
            return _noArgs.GetOrAdd(key, k => CreateInvoker(getMethodInfo));
        }

        private Func<T, TResult> CreateInvoker(Func<MethodInfo> getMethodInfo)
        {
            MethodInfo method = getMethodInfo();
            if (method == null) throw new FastReflectionException(ObjectType, "No available method.");
            ParameterExpression instanceParameter = Expression.Parameter(typeof(T), "target");
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method)
                : Expression.Call(instanceParameter, method);
            UnaryExpression callCast = typeof(TResult).IsValueType
                ? Expression.Convert(call, typeof(TResult))
                : Expression.TypeAs(call, typeof(TResult));
            return Expression.Lambda<Func<T, TResult>>(callCast, instanceParameter).Compile();
        }

        private Func<T, object[], TResult> GetInvoker(int key, Func<MethodInfo> getMethodInfo, object[] args)
        {
            return _withArgs.GetOrAdd(key, k => CreateInvoker(getMethodInfo, args));
        }

        private Func<T, object[], TResult> CreateInvoker(Func<MethodInfo> getMethodInfo, object[] args)
        {
            MethodInfo method = getMethodInfo();
            if (method == null) throw new FastReflectionException(ObjectType, "No available method.");
            ParameterExpression instanceParameter = Expression.Parameter(typeof(T), "target");
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression[] parameters = method.GetParameters().ToArrayIndexParameters(argsParameter).ToArray();
            MethodCallExpression call = method.IsStatic
                ? Expression.Call(method, parameters)
                : Expression.Call(instanceParameter, method, parameters);
            UnaryExpression callCast = typeof(TResult).IsValueType
                ? Expression.Convert(call, typeof(TResult))
                : Expression.TypeAs(call, typeof(TResult));
            return
                Expression.Lambda<Func<T, object[], TResult>>(callCast, instanceParameter, argsParameter).Compile();
        }

        #endregion
    }
}