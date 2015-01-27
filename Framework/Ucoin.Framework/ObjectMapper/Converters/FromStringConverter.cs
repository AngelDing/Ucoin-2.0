﻿using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FromStringConverter : Converter
    {
        private static readonly MethodInfo _enumParseMethod = typeof (Enum).GetMethod("Parse",
            BindingFlags.Public | BindingFlags.Static, null, new[] {typeof (Type), typeof (string)}, null);

        private static readonly MethodInfo _stringTrimMethod = typeof (string).GetMethod("Trim",
            BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

        private static readonly MethodInfo _checkEmptyMethod = typeof (string).GetMethod("IsNullOrWhiteSpace",
            BindingFlags.Public | BindingFlags.Static, null, new[] {typeof (string)}, null);

        private static readonly ConcurrentDictionary<Type, MethodInfo> _methods =
            new ConcurrentDictionary<Type, MethodInfo>();

        private static MethodInfo GetConvertMethod(Type targetType)
        {
            return _methods.GetOrAdd(targetType, FindConvertMethod);
        }

        private static MethodInfo FindConvertMethod(Type type)
        {
            return type.IsEnum
                ? _enumParseMethod
                : type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
        }

        public override int Match(ConverterMatchContext context)
        {
            if (context.SourceType == typeof (string))
            {
                Type targetType = context.TargetType;
                if (targetType.IsNullable() && GetConvertMethod(targetType.GetGenericArguments()[0]) != null) return 1;
                if (GetConvertMethod(context.TargetType) != null) return 0;
            }
            return -1;
        }

        public override void Compile(ModuleBuilder builder)
        {
        }

        public override void Emit(Type sourceType, Type targetType, CompilationContext context)
        {
            LocalBuilder target = context.DeclareLocal(targetType);
            LocalBuilder local = context.DeclareLocal(sourceType);
            context.Emit(OpCodes.Stloc, local);

            if (targetType.IsNullable())
            {
                Label labelEnd = context.DefineLabel();
                Label labelFirst = context.DefineLabel();
                // if(source == null)
                context.Emit(OpCodes.Ldloc, local);
                context.Emit(OpCodes.Brtrue, labelFirst);

                // target = null;
                context.Emit(OpCodes.Ldloca, target);
                context.Emit(OpCodes.Initobj, targetType);

                // goto end;
                context.Emit(OpCodes.Br, labelEnd);
                context.MakeLabel(labelFirst);

                // if(string.IsNullOrWhiteSpace(source))
                Label labelSecond = context.DefineLabel();
                context.Emit(OpCodes.Ldloc, local);
                context.EmitCall(_checkEmptyMethod);
                context.Emit(OpCodes.Brfalse, labelSecond);

                // target = new Nullable<T>(default(T));
                context.EmitDefault(targetType.GetGenericArguments()[0]);
                context.Emit(OpCodes.Newobj, targetType.GetConstructors()[0]);
                context.Emit(OpCodes.Stloc, target);

                // goto end;
                context.Emit(OpCodes.Br, labelEnd);
                context.MakeLabel(labelSecond);

                // target = new Nullable<$EnumType$>(($EnumType$)Enum.Parse(typeof($EnumType$),source));
                // or
                // target = new Nullable<$TargetType$>($TargetType$.Parse(source));
                Type underlingType = targetType.GetGenericArguments()[0];
                if (underlingType.IsEnum)
                {
                    context.EmitTypeOf(underlingType);
                }
                context.Emit(OpCodes.Ldloc, local);
                context.EmitCall(_stringTrimMethod);
                context.EmitCall(GetConvertMethod(underlingType));
                context.EmitCast(underlingType);
                context.Emit(OpCodes.Newobj, targetType.GetConstructors()[0]);
                context.Emit(OpCodes.Stloc, target);

                context.MakeLabel(labelEnd);
                context.Emit(OpCodes.Ldloc, target);
            }
            else
            {
                // if(!string.IsNullOrWhiteSpace(source))
                Label label = context.DefineLabel();
                context.Emit(OpCodes.Ldloc, local);
                context.EmitCall(_checkEmptyMethod);
                context.Emit(OpCodes.Brtrue, label);

                // target = ($EnumType$)Enum.Parse(typeof($EnumType$),source);
                // or
                // target = $TargetType$.Parse(source);
                if (targetType.IsEnum)
                {
                    context.EmitTypeOf(targetType);
                }
                context.Emit(OpCodes.Ldloc, local);
                context.EmitCall(_stringTrimMethod);
                context.EmitCall(GetConvertMethod(targetType));
                context.EmitCast(targetType);
                context.Emit(OpCodes.Stloc, target);

                // goto end;
                Label labelEnd = context.DefineLabel();
                context.Emit(OpCodes.Br, labelEnd);

                // target = default(T);
                context.MakeLabel(label);
                context.EmitDefault(targetType);
                context.Emit(OpCodes.Stloc, target);

                context.MakeLabel(labelEnd);
                context.Emit(OpCodes.Ldloc, target);
            }
        }
    }
}