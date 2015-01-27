using System;

namespace Ucoin.Framework.ObjectMapper
{
    internal class ConverterKey
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as ConverterKey;
            if (other == null) return false;
            return SourceType == other.SourceType && TargetType == other.TargetType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (SourceType.GetHashCode() * 397) ^ TargetType.GetHashCode();
            }
        }

        public ConverterKey(Type sourceType, Type targetType)
        {
            SourceType = sourceType;
            TargetType = targetType;
        }

        public Type SourceType { get; private set; }

        public Type TargetType { get; private set; }
    }
}
