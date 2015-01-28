using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class FuncInvokerBuilder<T, TResult> : IInvokerBuilder
    {
        private readonly Func<T, TResult> _func;
        private MethodInfo _invokeMethod;

        public FuncInvokerBuilder(Func<T, TResult> func)
        {
            _func = func;
        }

        public void Compile(ModuleBuilder builder)
        {
            TypeBuilder typeBuilder = builder.DefineStaticType();
            FieldBuilder field = typeBuilder.DefineStaticField<Func<T, TResult>>("Target");
            MethodBuilder methodBuilder = typeBuilder.DefineStaticMethod("Invoke");
            methodBuilder.SetReturnType(typeof (TResult));
            methodBuilder.SetParameters(typeof (T));

            ILGenerator il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldsfld, field);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Callvirt, typeof (Func<T, TResult>).GetMethod("Invoke"));
            il.Emit(OpCodes.Ret);
            Type type = typeBuilder.CreateType();
            type.FastSetField("Target", _func);
            _invokeMethod = type.GetMethod("Invoke");
        }

        public void Emit(CompilationContext context)
        {
            context.EmitCall(_invokeMethod);
            context.CurrentType = typeof (TResult);
        }
    }
}