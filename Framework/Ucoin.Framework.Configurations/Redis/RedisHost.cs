using System;
using System.Configuration;

namespace Ucoin.Framework.Configurations
{
	public class RedisHost : ConfigurationElement
	{
		[ConfigurationProperty("ip", IsRequired = true)]
		public string IP
		{
			get
			{
				return this["ip"] as string; 
			}
		}

		[ConfigurationProperty("port", IsRequired = true)]
		public int Port
		{
			get
			{
                var config = this["port"];
				if (config != null)
				{
					var value = config.ToString();

					if (!string.IsNullOrEmpty(value))
					{
						int result;

						if (int.TryParse(value, out result))
						{
							return result;
						}
					}
				}

				throw new Exception("Redis Cahe port must be number.");
			}
		}

        [ConfigurationProperty("isReadonly", IsRequired = true)]
        public bool IsReadonly
        {
            get
            {
                return (bool)this["isReadonly"];
            }
        }

        public string HostFullName
        {
            get
            {
                return string.Format("{0}:{1}", this.IP, this.Port);
            }
        }
	}
}