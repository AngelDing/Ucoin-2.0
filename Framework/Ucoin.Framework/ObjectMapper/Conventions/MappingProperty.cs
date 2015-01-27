using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Ucoin.Framework.ObjectMapper
{
    internal class MappingProperty : MappingMember
    {
        private readonly PropertyInfo _property;

        public MappingProperty(PropertyInfo property)
        {
            _property = property;
        }

        public override string MemberName
        {
            get { return _property.Name; }
        }

        public override Type MemberType
        {
            get { return _property.PropertyType; }
        }

        public override MemberInfo ClrMember
        {
            get { return _property; }
        }

        public override bool CanRead(bool includeNonPublic)
        {
            return _property.GetGetMethod(includeNonPublic) != null;
        }

        public override bool CanWrite(bool includeNonPublic)
        {
            return _property.GetSetMethod(includeNonPublic) != null;
        }

        internal override void EmitSetter(CompilationContext context)
        {
            LocalBuilder local = context.DeclareLocal(context.CurrentType);
            context.Emit(OpCodes.Stloc, local);

            MethodInfo setMethod = _property.GetSetMethod(true);
            context.LoadTarget();
            context.Emit(OpCodes.Ldloc, local);
            if (MemberType != context.CurrentType)
            {
                context.EmitCast(MemberType);
            }
            context.EmitCall(setMethod);
            context.CurrentType = null;
        }

        internal override void EmitGetter(CompilationContext context)
        {
            MethodInfo getMethod = _property.GetGetMethod(true);
            context.LoadSource();
            context.EmitCall(getMethod);
            context.CurrentType = MemberType;
        }
    }
}