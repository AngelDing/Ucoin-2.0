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

        public string Serialize(object input)
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

        public T Deserialize<T>(string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input, GetSettings());
            }
            catch
            {
                return default(T);
            }
        }

        public object Deserialize(string input, System.Type type)
        {
            try
            {
                return JsonConvert.DeserializeObject(input, GetSettings());
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
