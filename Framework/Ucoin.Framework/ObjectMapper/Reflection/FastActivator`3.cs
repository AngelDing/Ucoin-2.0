using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastActivator<T, TArg0, TArg1> : FastActivatorBase
    {
        [ThreadStatic]
        private static FastActivator<T, TArg0, TArg1> _current;
        private Lazy<Func<TArg0, TArg1, T>> _creator;

        private void Initialize(ConstructorInfo constructor)
        {
            _creator = new Lazy<Func<TArg0, TArg1, T>>(() => CreateCreator(constructor));
        }

        private T CreateInstance(TArg0 arg0, TArg1 arg1)
        {
            return _creator.Value(arg0, arg1);
        }

        private FastActivator()
            : base(typeof(T))
        {
            ConstructorInfo constructor = Constructors
                .MatchingArguments<ConstructorInfo, TArg0, TArg1>()
                .FirstOrDefault();

            if (constructor == null)
                throw new FastReflectionException(typeof(T), "No usable constructor found", typeof(TArg0),
                    typeof(TArg1));
            Initialize(constructor);
        }

        private Func<TArg0, TArg1, T> CreateCreator(ConstructorInfo constructor)
        {
            ParameterExpression[] parameters =
                constructor.GetParameters()
                    .Select((parameter, index) => Expression.Parameter(parameter.ParameterType, parameter.Name ?? ("arg" + index)))
                    .ToArray();

            return Expression.Lambda<Func<TArg0, TArg1, T>>(Expression.New(constructor, parameters), parameters).Compile();
        }

        private static FastActivator<T, TArg0, TArg1> Current
        {
            get { return _current ?? (_current = new FastActivator<T, TArg0, TArg1>()); }
        }

        public static T Create(TArg0 arg0, TArg1 arg1)
        {
            return Current.CreateInstance(arg0, arg1);
        }
    }
}