using System.Linq;
using Ucoin.Framework.Dependency;

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
                return SimpleLocator<SerializerLocator>.Current.Resolve<JsonSerializer>();
            }
        }

        public static ISerializer Xml
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<XmlSerializer>();
            }
        }

        public static ISerializer Binary
        {
            get
            {
                return SimpleLocator<SerializerLocator>.Current.Resolve<BinarySerializer>();
            }
        }
    }
}
