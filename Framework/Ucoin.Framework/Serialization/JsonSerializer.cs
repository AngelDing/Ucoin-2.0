using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Ucoin.Framework.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        public object Serialize(object input)
        {
            try
            {
                return JsonConvert.SerializeObject(input, GetSettings());
            }
            catch
            {
                return string.Empty;
            }
        }

        public T Deserialize<T>(object input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input.ToString(), GetSettings());
            }
            catch
            {
                return default(T);
            }
        }

        public object Deserialize(object input, System.Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(input.ToString(), GetSettings());
            }
            catch
            {
                return null;
            }
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters = new JsonConverter[] { new StringEnumConverter() };
            return settings;
        }
    }
}
