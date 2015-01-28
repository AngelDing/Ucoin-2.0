using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class DefaultCreator<TTarget> : IInstanceCreator<TTarget>
    {
        public void Compile(ModuleBuilder builder)
        {
        }

        public void Emit(CompilationContext context)
        {
            if (typeof (TTarget).IsValueType || typeof (TTarget).IsNullable())
            {
                LocalBuilder targetLocal = context.DeclareLocal(typeof (TTarget));
                context.Emit(OpCodes.Ldloca, targetLocal);
                context.Emit(OpCodes.Initobj, typeof (TTarget));
                context.Emit(OpCodes.Ldloc, targetLocal);
            }
            else
            {
                ConstructorInfo constructor =
                    typeof (TTarget).GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                        Type.EmptyTypes, null);
                if (constructor == null)
                {
                    throw new ArgumentException(string.Format("Type '{0}' does not have a default constructor.",
                        typeof (TTarget)));
                }
                context.Emit(OpCodes.Newobj, constructor);
            }
            context.CurrentType = typeof (TTarget);
        }
    }
}