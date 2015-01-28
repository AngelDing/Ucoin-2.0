using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class NativeExtensions
    {
        public static Type ToSpecializedType<T>(this T method, object[] args)
            where T : MethodBase
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            Type type = method.DeclaringType;
            if (!type.IsGenericType)
                throw new ArgumentException("The argument must be for a generic type", "method");

            Type[] genericArguments = GetGenericTypesFromArguments(method.GetParameters(), type.GetGenericArguments(),
                args);

            return type.MakeGenericType(genericArguments);
        }

        public static MethodInfo ToSpecializedMethod(this MethodInfo method, object[] args)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            if (!method.IsGenericMethod)
                return method;
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            Type[] genericArguments = GetGenericTypesFromArguments(method.GetParameters(), method.GetGenericArguments(),
                args);

            return method.MakeGenericMethod(genericArguments);
        }

        public static MethodInfo ToSpecializedMethod(this MethodInfo method, Type[] genericTypes, object[] args)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            if (!method.IsGenericMethod)
                return method;
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }
            if (genericTypes == null)
            {
                throw new ArgumentNullException("genericTypes");
            }
            Type[] arguments = method.GetGenericArguments()
                .ApplyGenericTypesToArguments(genericTypes);

            arguments = GetGenericTypesFromArguments(method.GetParameters(), arguments, args);

            method = method.MakeGenericMethod(arguments);

            return method;
        }

        private static Type[] ApplyGenericTypesToArguments(this Type[] arguments, Type[] genericTypes)
        {
            for (int i = 0; i < arguments.Length && i < genericTypes.Length; i++)
            {
                if (genericTypes[i] != null)
                    arguments[i] = genericTypes[i];
            }

            return arguments;
        }

        private static Type[] GetGenericTypesFromArguments(IEnumerable<ParameterInfo> parameterInfos, Type[] arguments,
            object[] args)
        {
            var parameters = parameterInfos
                .Merge(args, (x, y) => new {Parameter = x, Argument = y}).ToArray();

            for (int i = 0; i < arguments.Length; i++)
            {
                Type argumentType = arguments[i];

                if (!argumentType.IsGenericParameter)
                    continue;
                int index = i;
                parameters
                    .Where(arg => arg.Parameter.ParameterType == argumentType && arg.Argument != null)
                    .Select(arg => arg.Argument.GetType())
                    .ForEach(type =>
                    {
                        arguments[index] = type;

                        var more = argumentType.GetGenericParameterConstraints()
                            .Where(x => x.IsGenericType)
                            .Where(x => type.Implements(x.GetGenericTypeDefinition()))
                            .SelectMany(x => x.GetGenericArguments()
                                .Merge(type.GetGenericTypeDeclarations(x.GetGenericTypeDefinition()),
                                    (c, a) => new {Argument = c, Type = a}));

                        foreach (var next in more)
                        {
                            for (int j = 0; j < arguments.Length; j++)
                            {
                                if (arguments[j] == next.Argument)
                                    arguments[j] = next.Type;
                            }
                        }
                    });

                foreach (
                    var parameter in
                        parameters.Where(x => x.Parameter.ParameterType.IsGenericType && x.Argument != null))
                {
                    Type definition = parameter.Parameter.ParameterType.GetGenericTypeDefinition();
                    IEnumerable<Type> declaredTypesForGeneric =
                        parameter.Argument.GetType().GetGenericTypeDeclarations(definition);

                    var mergeds = parameter.Parameter.ParameterType.GetGenericArguments()
                        .Merge(declaredTypesForGeneric, (p, a) => new {ParameterType = p, ArgumentType = a});

                    foreach (var merged in mergeds)
                    {
                        for (int j = 0; j < arguments.Length; j++)
                        {
                            if (arguments[j] == merged.ParameterType)
                                arguments[j] = merged.ArgumentType;
                        }
                    }
                }
            }

            return arguments;
        }

        public static IEnumerable<Expression> ToArrayIndexParameters(this IEnumerable<ParameterInfo> parameters,
            ParameterExpression arguments)
        {
            return parameters.Select((parameter, index) =>
            {
                BinaryExpression arrayExpression = Expression.ArrayIndex(arguments, Expression.Constant(index));
                return parameter.ParameterType.IsValueType
                    ? (Expression)Expression.Convert(arrayExpression, parameter.ParameterType)
                    : (Expression)Expression.TypeAs(arrayExpression, parameter.ParameterType);
            });
        }
    }
}