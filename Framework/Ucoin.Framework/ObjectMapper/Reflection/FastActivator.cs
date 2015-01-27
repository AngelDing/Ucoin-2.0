using System;
using System.Collections.Concurrent;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FastActivator
    {
        [ThreadStatic]
        private static FastActivator _current;

        private readonly ConcurrentDictionary<Type, IFastActivator> _activators =
            new ConcurrentDictionary<Type, IFastActivator>();

        private IFastActivator GetActivator(Type type)
        {
            return _activators.GetOrAdd(type, CreateActivator);
        }

        private IFastActivator GetGenericActivator(Type type)
        {
            return _activators.GetOrAdd(type, CreateGenericActivator);
        }

        private IFastActivator CreateActivator(Type type)
        {
            return (IFastActivator)typeof(FastActivator<>).MakeGenericType(type).FastGetProperty("Current");
        }

        private IFastActivator CreateGenericActivator(Type type)
        {
            return new GenericFastActivator(type);
        }

        private FastActivator()
        {
        }

        private static FastActivator Current
        {
            get { return _current ?? (_current = new FastActivator()); }
        }

        public static T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        public static T Create<T, TArg0>(TArg0 arg0)
        {
            return (T)Create(typeof(T), arg0);
        }

        public static T Create<T, TArg0, TArg1>(TArg0 arg0, TArg1 arg1)
        {
            return (T)Create(typeof(T), arg0, arg1);
        }

        public static T Create<T>(params object[] args)
        {
            return (T)Create(typeof(T), args);
        }

        public static object Create(Type type)
        {
            return Current.GetActivator(type).Create();
        }

        public static object Create<TArg0>(Type type, TArg0 arg0)
        {
            if (type.IsGenericTypeDefinition)
                Current.GetGenericActivator(type).Create(arg0);

            return Current.GetActivator(type).Create(arg0);
        }

        public static object Create<TArg0, TArg1>(Type type, TArg0 arg0, TArg1 arg1)
        {
            if (type.IsGenericTypeDefinition)
                Current.GetGenericActivator(type).Create(arg0, arg1);

            return Current.GetActivator(type).Create(arg0, arg1);
        }

        public static object Create(Type type, object[] args)
        {
            if (type.IsGenericTypeDefinition)
                Current.GetGenericActivator(type).Create(args);

            return Current.GetActivator(type).Create(args);
        }

        public static object Create(Type type, Type[] genericTypes)
        {
            Type genericType = GetGenericType(type, genericTypes);

            return Current.GetActivator(genericType).Create();
        }

        public static object Create(Type type, Type[] genericTypes, object[] args)
        {
            Type genericType = GetGenericType(type, genericTypes);

            return Current.GetActivator(genericType).Create(args);
        }

        private static Type GetGenericType(Type type, Type[] genericTypes)
        {
            if (!type.IsGenericTypeDefinition)
                throw new ArgumentException("The type specified must be a generic type");

            Type[] genericArguments = type.GetGenericArguments();

            if (genericArguments.Length != genericTypes.Length)
            {
                throw new ArgumentException("An incorrect number of generic arguments was specified: " +
                                            genericTypes.Length
                                            + " (needed " + genericArguments.Length + ")");
            }

            return type.MakeGenericType(genericTypes);
        }
    }
}