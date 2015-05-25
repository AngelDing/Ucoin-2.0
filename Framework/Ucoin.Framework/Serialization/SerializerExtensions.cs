
namespace Ucoin.Framework.Serialization
{
    public static class SerializerExtensions
    {
        public static string SerializeToString(this ISerializer serializer, object data)
        {
            var res = serializer.Serialize(data);
            var format = serializer.Format;
            if (format == SerializationFormat.Json || format == SerializationFormat.Xml)
            {
                if (res != null)
                {
                    return res.ToString();
                }
            }

            return string.Empty;
        }
    }
}
