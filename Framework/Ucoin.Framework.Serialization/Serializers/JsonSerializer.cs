using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ucoin.Framework.Serialization
{
    /// <summary>
    /// 支持循環引用
    /// </summary>
    public class JsonSerializer : BaseSerializer<JsonSerializer>, ISerializer
    {
        internal override object DoDeserialize(object serializedObject, Type type)
        {
            return JsonConvert.DeserializeObject(serializedObject.ToString(), GetSettings());
        }

        internal override T DoDeserialize<T>(object serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject.ToString(), GetSettings());
        }

        internal override object DoSerialize(object item)
        {
            return JsonConvert.SerializeObject(item, GetSettings());
        }

        internal override SerializationFormat GetSerializationFormat()
        {
            return SerializationFormat.Json;
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters = new JsonConverter[] { new StringEnumConverter() };
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }
    }
}
