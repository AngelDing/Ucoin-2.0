﻿using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class ActionInvokerBuilder<TSource, TTarget> : IInvokerBuilder
    {
        private readonly Action<TSource, TTarget> _action;
        private MethodInfo _invokeMethod;

        public ActionInvokerBuilder(Action<TSource, TTarget> action)
        {
            _action = action;
        }

        public void Compile(ModuleBuilder builder)
        {
            TypeBuilder typeBuilder = builder.DefineStaticType();
            FieldBuilder field = typeBuilder.DefineStaticField<Action<TSource, TTarget>>("Target");
            MethodBuilder methodBuilder = typeBuilder.DefineStaticMethod("Invoke");
            methodBuilder.SetParameters(typeof (TSource), typeof (TTarget));

            ILGenerator il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldsfld, field);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof (Action<TSource, TTarget>).GetMethod("Invoke"));
            il.Emit(OpCodes.Ret);
            Type type = typeBuilder.CreateType();
            type.FastSetField("Target", _action);
            _invokeMethod = type.GetMethod("Invoke");
        }

        public void Emit(CompilationContext context)
        {
            context.EmitCall(_invokeMethod);
            context.CurrentType = null;
        }
    }
}