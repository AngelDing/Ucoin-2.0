using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class ReflectionExtensions
    {
        #region Method Matching

        public static IEnumerable<T> MatchingArguments<T>(this IEnumerable<T> methods)
            where T : MethodBase
        {
            return methods
                .Where(x => x.GetParameters().MatchesArguments());
        }

        public static IEnumerable<T> MatchingArguments<T, TArg0>(this IEnumerable<T> methods)
            where T : MethodBase
        {
            return methods
                .Where(x => x.GetParameters().MatchesArguments<TArg0>());
        }

        public static IEnumerable<T> MatchingArguments<T, TArg0, TArg1>(this IEnumerable<T> methods)
            where T : MethodBase
        {
            return methods
                .Where(x => x.GetParameters().MatchesArguments<TArg0, TArg1>());
        }

        public static IEnumerable<T> MatchingArguments<T>(this IEnumerable<T> methods, object[] args)
            where T : MethodBase
        {
            return methods
                .Select(x => new {Method = x, Rating = x.GetParameters().MatchesArguments(args)})
                .Where(x => x.Rating > 0)
                .OrderByDescending(x => x.Rating)
                .Select(x => x.Method);
        }

        public static IEnumerable<MethodInfo> MatchingArguments(this IEnumerable<MethodInfo> methods,
            Type[] genericTypes, object[] args)
        {
            return methods
                .Select(
                    x =>
                        new
                        {
                            Method = x,
                            Rating = x.ToSpecializedMethod(genericTypes, args).GetParameters().MatchesArguments(args)
                        })
                .Where(x => x.Rating > 0)
                .OrderByDescending(x => x.Rating)
                .Select(x => x.Method);
        }

        private static bool MatchesArguments(this IEnumerable<ParameterInfo> parameters)
        {
            return !parameters.Any();
        }

        private static bool MatchesArguments<TArg0>(this IEnumerable<ParameterInfo> parameters)
        {
            ParameterInfo[] args = parameters.ToArray();

            return args.Length == 1
                   && args[0].ParameterType.RateParameterTypeCompatibility(typeof (TArg0)) > 0;
        }

        private static bool MatchesArguments<TArg0, TArg1>(this IEnumerable<ParameterInfo> parameters)
        {
            ParameterInfo[] args = parameters.ToArray();

            return args.Length == 2
                   && args[0].ParameterType.RateParameterTypeCompatibility(typeof (TArg0)) > 0
                   && args[1].ParameterType.RateParameterTypeCompatibility(typeof (TArg1)) > 0;
        }

        private static int MatchesArguments(this ParameterInfo[] parameterInfos, object[] args)
        {
            if (parameterInfos.Length != args.Length)
                return 0;

            if (parameterInfos.Length == 0)
                return 23;

            var matched = parameterInfos.Merge(args, (x, y) => new
            {
                Parameter = x,
                Argument = y,
                Rating = RateParameterTypeCompatibility(x.ParameterType, y)
            }).ToArray();

            int valid = matched.Count(x => x.Rating > 0);

            if (valid != args.Length)
                return 0;

            return matched.Sum(x => x.Rating);
        }

        private static int RateParameterTypeCompatibility(this Type parameterType, object arg)
        {
            if (arg == null)
                return parameterType.CanBeNull() ? 1 : 0;

            return parameterType.RateParameterTypeCompatibility(arg.GetType());
        }

        private static bool CanBeNull(this Type type)
        {
            return !type.IsValueType || type.IsNullableType() || type == typeof (string);
        }

        private static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        private static int RateParameterTypeCompatibility(this Type parameterType, Type argType)
        {
            if (argType == parameterType)
                return 22;

            if (parameterType.IsGenericParameter)
                return argType.MeetsGenericConstraints(parameterType) ? 21 : 0;

            if (parameterType.IsGenericType)
            {
                Type definition = parameterType.GetGenericTypeDefinition();

                if (argType.IsGenericType)
                {
                    int matchDepth = parameterType.GetMatchDepth(argType);
                    if (matchDepth > 0)
                        return matchDepth + 5;
                }

                if (argType.Implements(definition))
                    return parameterType.IsInterface ? 4 : 5;
            }

            if (parameterType.IsAssignableFrom(argType))
            {
                // favor base class over interface
                return parameterType.IsInterface ? 2 : 3;
            }

            return 0;
        }

        private static bool MeetsGenericConstraints(this Type type, Type genericType)
        {
            Type[] constraints = genericType.GetGenericParameterConstraints();

            int matched = constraints.Count(x => type.Implements(x.GetGenericTypeDefinition()));

            return matched == constraints.Length;
        }

        private static int GetMatchDepth(this Type type, Type targetType)
        {
            if (!type.IsGenericType || !targetType.IsGenericType)
                return 0;

            Type typeGeneric = type.GetGenericTypeDefinition();
            Type targetTypeGeneric = targetType.GetGenericTypeDefinition();

            if (typeGeneric != targetTypeGeneric)
                return 0;

            int result = type
                .GetGenericArguments()
                .MergeBalanced(targetType.GetGenericArguments(), (x, y) => new {Type = x, TargetType = y})
                .Select(x => x.Type.GetMatchDepth(x.TargetType))
                .Sum();

            return result + 1;
        }

        #endregion

        #region Implement Interface

        /// <summary>
        ///     Checks if a type implements an open generic at any level of the inheritance chain, including all
        ///     base classes
        /// </summary>
        /// <param name="objectType">The type to check</param>
        /// <param name="interfaceType">The interface type (must be a generic type definition)</param>
        /// <param name="matchedType">The matching type that was found for the interface type</param>
        /// <returns>True if the interface is implemented by the type, otherwise false</returns>
        public static bool ImplementsGeneric(this Type objectType, Type interfaceType, out Type matchedType)
        {
            matchedType = null;

            if (interfaceType.IsInterface)
            {
                matchedType =
                    objectType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
                if (matchedType != null)
                    return true;
            }

            if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == interfaceType)
            {
                matchedType = objectType;
                return true;
            }

            Type baseType = objectType.BaseType;
            if (baseType == null)
                return false;

            return baseType.ImplementsGeneric(interfaceType, out matchedType);
        }

        /// <summary>
        ///     Checks if a type implements an open generic at any level of the inheritance chain, including all
        ///     base classes
        /// </summary>
        /// <param name="objectType">The type to check</param>
        /// <param name="interfaceType">The interface type (must be a generic type definition)</param>
        /// <returns>True if the interface is implemented by the type, otherwise false</returns>
        public static bool ImplementsGeneric(this Type objectType, Type interfaceType)
        {
            Type matchedType;
            return objectType.ImplementsGeneric(interfaceType, out matchedType);
        }

        /// <summary>
        ///     Checks if a type implements the specified interface
        /// </summary>
        /// <param name="objectType">The type to check</param>
        /// <param name="interfaceType">The interface type (can be generic, either specific or open)</param>
        /// <returns>True if the interface is implemented by the type, otherwise false</returns>
        public static bool Implements(this Type objectType, Type interfaceType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }
            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }

            if (interfaceType.IsGenericTypeDefinition)
                return objectType.ImplementsGeneric(interfaceType);

            return interfaceType.IsAssignableFrom(objectType);
        }

        /// <summary>
        ///     Checks if a type implements the specified interface
        /// </summary>
        /// <typeparam name="T">The interface type (can be generic, either specific or open)</typeparam>
        /// <param name="objectType">The type to check</param>
        /// <returns>True if the interface is implemented by the type, otherwise false</returns>
        public static bool Implements<T>(this Type objectType)
        {
            return objectType.Implements(typeof (T));
        }

        #endregion

        public static IEnumerable<Type> GetGenericTypeDeclarations(this Type objectType, Type genericType)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }
            if (genericType == null)
            {
                throw new ArgumentNullException("genericType");
            }
            if (!genericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Must be an open generic type", "genericType");
            }

            Type matchedType;
            if (objectType.ImplementsGeneric(genericType, out matchedType))
            {
                foreach (Type argument in matchedType.GetGenericArguments())
                {
                    yield return argument;
                }
            }
        }
    }
}