using System;
using System.IO;

namespace Ucoin.Framework.Serialization
{
    /// <summary>
    /// 1.不支持循環引用;
    /// 2.不支持DateTimeOffset數據類型;
    /// </summary>
    public class ProtoBufSerializer : BaseSerializer<ProtoBufSerializer>, ISerializer
    {
        internal override object DoDeserialize(object serializedObject, Type type)
        {
            object result = null;
            using (var stream = new MemoryStream(serializedObject as byte[]))
            {
                result = ProtoBuf.Serializer.Deserialize<object>(stream);
            }
            return result;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            T result = default(T);
            using (var stream = new MemoryStream(serializedObject as byte[]))
            {
                result = ProtoBuf.Serializer.Deserialize<T>(stream);
            }
            return result;
        }

        internal override object DoSerialize(object item)
        {
            object result = null;
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, item);
                result = stream.ToArray();
            }
            return result;
        }

        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.ProtoBuf;
        }
    }
}
