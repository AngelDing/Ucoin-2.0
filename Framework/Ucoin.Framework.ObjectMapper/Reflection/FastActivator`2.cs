using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastActivator<T, TArg0> : FastActivatorBase
    {
        [ThreadStatic] 
        private static FastActivator<T, TArg0> _current;

        private Lazy<Func<TArg0, T>> _creator;

        private void Initialize(ConstructorInfo constructor)
        {
            _creator = new Lazy<Func<TArg0, T>>(() => CreateCreator(constructor));
        }

        private T CreateInstance(TArg0 arg0)
        {
            return _creator.Value(arg0);
        }

        private FastActivator()
            : base(typeof (T))
        {
            ConstructorInfo constructor = Constructors
                .MatchingArguments<ConstructorInfo, TArg0>()
                .FirstOrDefault();
            if (constructor == null)
            {
                throw new FastReflectionException(typeof (T), "No usable constructor found", typeof (TArg0));
            }
            Initialize(constructor);
        }

        private static Func<TArg0, T> CreateCreator(ConstructorInfo constructor)
        {
            ParameterInfo parameterInfo = constructor.GetParameters().First();
            ParameterExpression parameter = Expression.Parameter(parameterInfo.ParameterType, parameterInfo.Name ?? "x");
            return Expression.Lambda<Func<TArg0, T>>(Expression.New(constructor, parameter), parameter).Compile();
        }

        public static FastActivator<T, TArg0> Current
        {
            get { return _current ?? (_current = new FastActivator<T, TArg0>()); }
        }

        public static T Create(TArg0 arg0)
        {
            return Current.CreateInstance(arg0);
        }
    }
}