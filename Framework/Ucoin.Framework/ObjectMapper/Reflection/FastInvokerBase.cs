using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal abstract class FastInvokerBase
    {
        private readonly Dictionary<string, MethodInfo[]> _methodCache;

        protected FastInvokerBase(Type type)
        {
            ObjectType = type;
            _methodCache = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                           BindingFlags.NonPublic)
                .GroupBy(method => method.Name)
                .ToDictionary(group => group.Key, group => group.ToArray());
        }

        public Type ObjectType { get; private set; }

        protected MethodInfo[] GetMethods(string methodName)
        {
            MethodInfo[] methods;
            if (!_methodCache.TryGetValue(methodName, out methods))
            {
                methods = new MethodInfo[0];
            }
            return methods;
        }

        protected static int GetArgumentHashCode(int seed, object[] args)
        {
            int key = seed;
            for (int i = 0; i < args.Length; i++)
                key ^= args[i] == null ? 31*i : args[i].GetType().GetHashCode() << i;
            return key;
        }

        protected static int GetArgumentHashCode(int seed, Type[] genericTypes)
        {
            int key = seed;
            for (int i = 0; i < genericTypes.Length; i++)
                key ^= genericTypes[i] == null ? 27*i : genericTypes[i].GetHashCode()*101 << i;
            return key;
        }

        protected static int GetArgumentHashCode(int seed, Type[] genericTypes, object[] args)
        {
            int key = seed;
            for (int i = 0; i < genericTypes.Length; i++)
                key ^= genericTypes[i] == null ? 27*i : genericTypes[i].GetHashCode()*101 << i;
            for (int i = 0; i < args.Length; i++)
                key ^= args[i] == null ? 31*i : args[i].GetType().GetHashCode() << i;
            return key;
        }

        protected static MethodInfo GetGenericMethodFromTypes(MethodInfo method, Type[] genericTypes)
        {
            if (!method.IsGenericMethod)
                throw new ArgumentException("Generic types cannot be specified for a non-generic method: " + method.Name);

            Type[] genericArguments = method.GetGenericArguments();

            if (genericArguments.Length != genericTypes.Length)
            {
                throw new ArgumentException("An incorrect number of generic arguments was specified: " +
                                            genericTypes.Length
                                            + " (needed " + genericArguments.Length + ")");
            }

            method = method.GetGenericMethodDefinition().MakeGenericMethod(genericTypes);
            return method;
        }
    }
}