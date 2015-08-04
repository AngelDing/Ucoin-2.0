using Jil;
using System;

namespace Ucoin.Framework.Serialization
{
    /// <summary>
    /// 1.不支持循環引用；
    /// 2.集合不支持ICollection，但支持IList
    /// </summary>
    public class JilSerializer : BaseSerializer<JilSerializer>,ISerializer
	{     
        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Jil;
        }

        internal override object DoSerialize(object item)
        {
            var result = JSON.Serialize(item, new Options(includeInherited: true));
            return result;
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            var result = serializedObject.ToString();
            return JSON.Deserialize<T>(result);
        }

        internal override object DoDeserialize(object serializedObject, Type type)
        {
            var result = serializedObject.ToString();
            return JSON.Deserialize(result, type);
        }
    }
}