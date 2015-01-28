using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal class GenericFastActivator : FastActivatorBase, IFastActivator
    {
        private readonly ConcurrentDictionary<int, Func<object[], object>> _activators =
            new ConcurrentDictionary<int, Func<object[], object>>();

        private Func<object[], object> GetActivator(int key, params object[] args)
        {
            return _activators.GetOrAdd(key, new Func<int, Func<object[], object>>(k => CreateActivator(args)));
        }

        public GenericFastActivator(Type genericType)
            : base(genericType)
        {
        }

        object IFastActivator.Create()
        {
            throw new NotSupportedException();
        }

        object IFastActivator.Create(object[] args)
        {
            return CreateFromArgs(args);
        }

        object IFastActivator.Create<TArg0>(TArg0 arg0)
        {
            return CreateFromArgs(arg0);
        }

        object IFastActivator.Create<TArg0, TArg1>(TArg0 arg0, TArg1 arg1)
        {
            return CreateFromArgs(arg0, arg1);
        }

        private object CreateFromArgs(params object[] args)
        {
            return GetActivator(GenerateTypeKey(args), args)(args);
        }

        private Func<object[], object> CreateActivator(object[] args)
        {
            ConstructorInfo constructor = Constructors.MatchingArguments(args).FirstOrDefault();

            if (constructor == null)
                throw new FastReflectionException(ObjectType, "No usable constructor found");

            Type specializedType = constructor.ToSpecializedType(args);

            constructor = specializedType.GetConstructors().MatchingArguments(args).FirstOrDefault();

            if (constructor == null)
                throw new FastReflectionException(specializedType,
                    "Specialized constructor could not be used to build the object");
            ParameterExpression argsParameter = Expression.Parameter(typeof(object[]), "args");
            Expression[] parameters = constructor.GetParameters().ToArrayIndexParameters(argsParameter).ToArray();
            return
                Expression.Lambda<Func<object[], object>>(Expression.New(constructor, parameters), argsParameter)
                    .Compile();
        }

        private static int GenerateTypeKey(params object[] args)
        {
            int offset = 0;
            return args.Aggregate(0, (x, o) => x ^ (o.GetType().GetHashCode() << offset++));
        }
    }
}