using Newtonsoft.Json;

namespace Ucoin.Framework.MongoRepository.Manager
{
    public static class UnityHelper
    {
        public static string ToJson(this object input)
        {

            var json = JsonConvert.SerializeObject(input, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            });
            return json;
        }
    }
}
