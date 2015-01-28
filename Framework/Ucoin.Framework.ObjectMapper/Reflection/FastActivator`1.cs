using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastActivator<T> : FastActivatorBase, IFastActivator<T>
    {
        [ThreadStatic] 
        private static FastActivator<T> _current;

        private readonly Func<T> _creator;

        private readonly ConcurrentDictionary<int, Func<object[], T>> _argCreators =
            new ConcurrentDictionary<int, Func<object[], T>>();

        private FastActivator()
            : base(typeof (T))
        {
            ConstructorInfo constructor = Constructors.MatchingArguments().SingleOrDefault();

            _creator = constructor == null
                ? () => (T) FormatterServices.GetUninitializedObject(typeof (T))
                : Expression.Lambda<Func<T>>(Expression.New(constructor)).Compile();
        }

        private static FastActivator<T> Current
        {
            get { return _current ?? (_current = new FastActivator<T>()); }
        }

        object IFastActivator.Create()
        {
            return Create();
        }

        object IFastActivator.Create(object[] args)
        {
            return CreateFromArgs(args);
        }

        object IFastActivator.Create<TArg0>(TArg0 arg0)
        {
            return Create(arg0);
        }

        object IFastActivator.Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1)
        {
            return Create(arg0, arg1);
        }

        T IFastActivator<T>.Create()
        {
            return Create();
        }

        T IFastActivator<T>.Create(object[] args)
        {
            return CreateFromArgs(args);
        }

        T IFastActivator<T>.Create<TArg0>(TArg0 arg0)
        {
            return FastActivator<T, TArg0>.Create(arg0);
        }

        T IFastActivator<T>.Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1)
        {
            return FastActivator<T, TArg0, TArg1>.Create(arg0, arg1);
        }

        private T CreateFromArgs(object[] args)
        {
            if (args == null || args.Length == 0)
                return _creator();

            int offset = 0;
            int key = args.Aggregate(0, (x, o) => x ^ (o == null ? offset : o.GetType().GetHashCode() << offset++));
            Func<object[], T> creator = _argCreators.GetOrAdd(key, k => CreateActivator(args));
            return creator(args);
        }

        private Func<object[], T> CreateActivator(object[] args)
        {
            ConstructorInfo constructor = Constructors.MatchingArguments(args).FirstOrDefault();
            if (constructor == null)
                throw new FastReflectionException(typeof(T), "No usable constructor found");
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression[] parameters = constructor.GetParameters().ToArrayIndexParameters(argsParameter).ToArray();
            return Expression.Lambda<Func<object[], T>>(Expression.New(constructor, parameters), argsParameter).Compile();
        }

        public static T Create()
        {
            return Current._creator();
        }

        public static T Create(object[] args)
        {
            return Current.CreateFromArgs(args);
        }

        public static T Create<TArg0>(TArg0 arg0)
        {
            return FastActivator<T, TArg0>.Create(arg0);
        }

        public static T Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1)
        {
            return FastActivator<T, TArg0, TArg1>.Create(arg0, arg1);
        }
    }
}