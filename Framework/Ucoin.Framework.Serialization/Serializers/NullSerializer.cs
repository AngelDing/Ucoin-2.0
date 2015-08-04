using System;

namespace Ucoin.Framework.Serialization
{
    public class NullSerializer : BaseSerializer<NullSerializer>, ISerializer
    {       
        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Null;
        }

        internal override object DoSerialize(object item)
        {
            return null;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            return default(T);
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            return null;
        }
    }
}
