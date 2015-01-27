using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class CompilationContext
    {
        private readonly ILGenerator _il;
        private Action _sourceLoader;
        private Action _targetLoader;

        public CompilationContext(ILGenerator il)
        {
            _il = il;
        }

        public Type CurrentType { get; set; }

        public void SetTarget(Action targetLoader)
        {
            _targetLoader = targetLoader;
        }

        public void SetSource(Action sourceLoader)
        {
            _sourceLoader = sourceLoader;
        }

        public void LoadTarget()
        {
            _targetLoader();
        }

        public void LoadSource()
        {
            _sourceLoader();
        }

        public void Emit(OpCode opCode)
        {
            _il.Emit(opCode);
        }

        public void Emit(OpCode opCode, FieldInfo field)
        {
            _il.Emit(opCode, field);
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            _il.Emit(opcode, meth);
        }

        public void Emit(OpCode opcode, LocalBuilder local)
        {
            _il.Emit(opcode, local);
        }

        public void Emit(OpCode opcode, Label label)
        {
            _il.Emit(opcode, label);
        }

        public virtual void Emit(OpCode opcode, ConstructorInfo con)
        {
            _il.Emit(opcode, con);
        }

        public virtual void Emit(OpCode opcode, Type cls)
        {
            _il.Emit(opcode, cls);
        }

        public virtual LocalBuilder DeclareLocal(Type localType)
        {
            return _il.DeclareLocal(localType);
        }

        public virtual Label DefineLabel()
        {
            return _il.DefineLabel();
        }

        public virtual void MakeLabel(Label label)
        {
            _il.MarkLabel(label);
        }
    }
}