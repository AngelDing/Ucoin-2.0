using MsgPack.Serialization;
using System;
using System.IO;

namespace Ucoin.Framework.Serialization
{
    /// <summary>
    /// 不支持循環引用
    /// </summary>
    public class MsgPackSerializer : BaseSerializer<MsgPackSerializer>, ISerializer
    {
        internal override object DoDeserialize(object serializedObject, Type type)
        {
            throw new NotImplementedException();
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            using (MemoryStream stream = new MemoryStream(serializedObject as byte[]))
            {
                // Creates serializer.
                var serializer = SerializationContext.Default.GetSerializer<T>();
                // Pack obj to stream.
                return serializer.Unpack(stream);
            }
        }

        internal override object DoSerialize(object item)
        {
            object result = null;
            var serializer = SerializationContext.Default.GetSerializer(item.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Pack(stream, item);
                result = stream.ToArray();
            }
            return result;
        }

        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.MsgPack;
        }
    }
}
