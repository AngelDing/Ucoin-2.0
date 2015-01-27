using System;

namespace Ucoin.Framework.ObjectMapper
{
    internal class TypeMemberKey
    {
        public TypeMemberKey(Type type, string memberName)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (string.IsNullOrEmpty(memberName))
            {
                throw new ArgumentException("The name of member cannot be null or empty.", "memberName");
            }
            Type = type;
            MemberName = memberName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as TypeMemberKey;
            if (other == null) return false;
            return Type == other.Type && MemberName == other.MemberName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Type.GetHashCode();
                return ((hashCode << 5) + hashCode) ^ MemberName.GetHashCode();
            }
        }

        public Type Type { get; private set; }

        public string MemberName { get; private set; }
    }
}
