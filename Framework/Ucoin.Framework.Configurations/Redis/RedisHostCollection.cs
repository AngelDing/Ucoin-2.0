using System.Configuration;

namespace Ucoin.Framework.Configurations
{
	public class RedisHostCollection : ConfigurationElementCollection
	{
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

		protected override ConfigurationElement CreateNewElement()
		{
			return new RedisHost();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
            var config = ((RedisHost)element);
            return config.HostFullName;
		}

        protected override string ElementName
        {
            get { return "host"; }
        }
	}
}