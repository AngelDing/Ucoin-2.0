using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal interface IInstanceCreator<TTarget>
    {
        void Compile(ModuleBuilder builder);

        void Emit(CompilationContext context);
    }
}