using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ucoin.Framework.Serialization
{
    /// <summary>
    /// 支持循環引用
    /// </summary>
    public class BinarySerializer : BaseSerializer<BinarySerializer>, ISerializer
    {
        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Binary;
        }      

        internal override object DoSerialize(object value)
        {
            byte[] result;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, value);
                stream.Flush();
                result = stream.ToArray();
            }
            return result;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            T result = default(T);
            var obj = Deserialize(serializedObject, typeof(T));
            if (null != obj)
            {
                result = (T)obj;
            }
            return result;
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            object result;
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(serializedObject as byte[]))
            {
                result = formatter.Deserialize(stream);
            }
            return result;
        }
    }
}
