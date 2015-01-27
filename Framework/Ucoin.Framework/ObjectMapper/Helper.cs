using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal static class Helper
    {
        internal static TypeBuilder DefineStaticType(this ModuleBuilder builder)
        {
            return builder.DefineType(Guid.NewGuid().ToString("N"),
                TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
        }

        internal static MethodBuilder DefineStaticMethod(this TypeBuilder builder, string methodName)
        {
            return builder.DefineMethod(methodName,
                MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig);
        }

        internal static FieldBuilder DefineStaticField<T>(this TypeBuilder builder, string fieldName)
        {
            return builder.DefineField(fieldName, typeof(T), FieldAttributes.Public | FieldAttributes.Static);
        }

        internal static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        internal static MethodInfo GetConvertMethod(Type sourceType, Type targetType)
        {
            return targetType.GetMethod("op_Implicit", BindingFlags.Public | BindingFlags.Static, null,
                new[] { sourceType }, null) ??
                   sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                       .Where(method =>
                       {
                           if (method.IsSpecialName && method.Name == "op_Implicit" && method.ReturnType == targetType)
                           {
                               ParameterInfo[] parameters = method.GetParameters();
                               return parameters.Length == 1 && parameters[0].ParameterType == sourceType;
                           }
                           return false;
                       }).FirstOrDefault() ??
                   targetType.GetMethod("op_Explicit", BindingFlags.Public | BindingFlags.Static, null,
                       new[] { sourceType }, null) ??
                   sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                       .Where(method =>
                       {
                           if (method.IsSpecialName && method.Name == "op_Explicit" && method.ReturnType == targetType)
                           {
                               ParameterInfo[] parameters = method.GetParameters();
                               return parameters.Length == 1 && parameters[0].ParameterType == sourceType;
                           }
                           return false;
                       }).FirstOrDefault();
        }

        internal static int GetDistance(Type sourceType, Type targetType)
        {
            int distance = 0;
            while (sourceType != null)
            {
                if (sourceType == targetType)
                {
                    return distance;
                }
                if (sourceType == typeof(object))
                {
                    break;
                }
                distance++;
                sourceType = sourceType.BaseType;
            }
            return -1;
        }
    }
}