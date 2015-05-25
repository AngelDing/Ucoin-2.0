using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ucoin.Framework.Serialization
{
    public class BinarySerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.Binary; }
        }      

        public object Serialize(object value)
        {
            byte[] serialized;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, value);
                stream.Flush();
                serialized = stream.ToArray();
            }
            return serialized;
        }

        public object Deserialize(object serializedValue, Type type)
        {
            try
            {
                object deserialized;
                var formatter = new BinaryFormatter();
                using (var stream = new MemoryStream(serializedValue as byte[]))
                {
                    deserialized = formatter.Deserialize(stream);
                }
                return deserialized;
            }
            catch
            {
                return null;
            }
        }

        public T Deserialize<T>(object input)
        {
            T res = default(T);
            var obj = Deserialize(input, typeof(T));
            if (null != obj)
            {
                res = (T)obj;
            }
            return res;
        }
    }
}
