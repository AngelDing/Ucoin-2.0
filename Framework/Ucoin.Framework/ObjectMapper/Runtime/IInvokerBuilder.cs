using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal interface IInvokerBuilder
    {
        void Compile(ModuleBuilder builder);

        void Emit(CompilationContext context);
    }
}