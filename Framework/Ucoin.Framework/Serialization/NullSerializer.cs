using System;

namespace Ucoin.Framework.Serialization
{
    public class NullSerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.Null; }
        }

        public object Serialize(object input)
        {
            return null; ;
        }

        public T Deserialize<T>(object input)
        {
            return default(T);
        }

        public object Deserialize(object input, Type type)
        {
            return null;
        }
    }
}
