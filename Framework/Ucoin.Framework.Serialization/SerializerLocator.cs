
using Ucoin.Framework.Dependency;

namespace Ucoin.Framework.Serialization
{
    public class SerializerLocator : SimpleLocator
    {
        public override void RegisterDefaults(IContainer container)
        {
            container.Register<JsonSerializer>(() => new JsonSerializer());
            container.Register<XmlSerializer>(() => new XmlSerializer());
            container.Register<BinarySerializer>(() => new BinarySerializer());
            container.Register<JilSerializer>(() => new JilSerializer());
        }
    }
}
