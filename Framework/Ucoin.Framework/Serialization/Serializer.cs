using System.Linq;

namespace Ucoin.Framework.Serialization
{
    /// <summary>
    /// Wrapper for accessing ISerializer implementations
    /// </summary>
    public static class Serializer
    {
        public static ISerializer Json     
        {
            get 
            {
                return SerializerLocator.Current.Resolve<JsonSerializer>();
            }
        }

        public static ISerializer Xml
        {
            get
            {
                return SerializerLocator.Current.Resolve<XmlSerializer>();
            }
        }

        public static ISerializer Binary
        {
            get
            {
                return SerializerLocator.Current.Resolve<BinarySerializer>();
            }
        }
    }
}
