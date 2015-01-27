using System;
using System.Collections;

namespace Ucoin.Framework.ObjectMapper
{
    internal sealed class ConverterMatchContext
    {
        private readonly Type _sourceType;
        private readonly Type _targetType;
        private Hashtable _properties;

        public ConverterMatchContext(Type sourceType, Type targetType)
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }

        public Type SourceType
        {
            get { return _sourceType; }
        }

        public Type TargetType
        {
            get { return _targetType; }
        }

        public Hashtable Properties
        {
            get { return _properties ?? (_properties = new Hashtable()); }
        }
    }
}