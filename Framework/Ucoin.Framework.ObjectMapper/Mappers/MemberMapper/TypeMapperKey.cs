using System;

namespace Ucoin.Framework.ObjectMapper
{
    internal class TypeMapperKey
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as TypeMapperKey;
            if (other == null) return false;
            return Container == other.Container && SourceType == other.SourceType && TargetType == other.TargetType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Container.GetHashCode();
                hashCode = ((hashCode << 5) + hashCode) ^ SourceType.GetHashCode();
                hashCode = ((hashCode << 5) + hashCode) ^ TargetType.GetHashCode();
                return hashCode;
            }
        }

        public TypeMapperKey(ObjectMapper container, Type sourceType, Type targetType)
        {
            Container = container;
            SourceType = sourceType;
            TargetType = targetType;
        }

        public ObjectMapper Container { get; private set; }

        public Type SourceType { get; private set; }

        public Type TargetType { get; private set; }
    }
}
