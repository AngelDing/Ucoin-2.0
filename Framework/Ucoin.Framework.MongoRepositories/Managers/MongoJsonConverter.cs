using MongoDB.Bson;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ucoin.Framework.MongoDb.Managers
{
    public class MongoJsonConverter : JsonConverter
    {
        public override void WriteJson(
            Newtonsoft.Json.JsonWriter writer, object value, JsonSerializer serializer)
        {
            var cursor = (List<BsonDocument>)value;
            var settings = new JsonWriterSettings
            {
                OutputMode = JsonOutputMode.Strict
            };

            writer.WriteStartArray();

            foreach (BsonDocument document in cursor)
            {
                writer.WriteRawValue(document.ToJson(settings));
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            var types = new[] { typeof(List<BsonDocument>) };
            return types.Any(t => t == objectType);
        }
    }
}
