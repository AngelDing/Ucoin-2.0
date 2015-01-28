using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class MappingField : MappingMember
    {
        private readonly FieldInfo _field;

        public MappingField(FieldInfo field)
        {
            _field = field;
        }

        public override string MemberName
        {
            get { return _field.Name; }
        }

        public override Type MemberType
        {
            get { return _field.FieldType; }
        }

        public override MemberInfo ClrMember
        {
            get { return _field; }
        }

        public override bool CanRead(bool includeNonPublic)
        {
            return includeNonPublic || _field.IsPublic;
        }

        public override bool CanWrite(bool includeNonPublic)
        {
            return (includeNonPublic || _field.IsPublic) && !(_field.IsLiteral || _field.IsInitOnly);
        }

        internal override void EmitSetter(CompilationContext context)
        {
            LocalBuilder local = context.DeclareLocal(context.CurrentType);
            context.Emit(OpCodes.Stloc, local);
            context.LoadTarget();
            context.Emit(OpCodes.Ldloc, local);
            if (MemberType != context.CurrentType)
            {
                context.EmitCast(MemberType);
            }
            context.Emit(OpCodes.Stfld, _field);
            context.CurrentType = null;
        }

        internal override void EmitGetter(CompilationContext context)
        {
            context.LoadSource();
            context.Emit(OpCodes.Ldfld, _field);
            context.CurrentType = _field.FieldType;
        }
    }
}