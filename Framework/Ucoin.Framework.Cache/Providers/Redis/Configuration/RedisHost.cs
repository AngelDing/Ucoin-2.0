﻿using System;
using System.Configuration;

namespace Ucoin.Framework.Cache
{
	public class RedisHost : ConfigurationElement
	{
		[ConfigurationProperty("host", IsRequired = true)]
		public string Host
		{
			get
			{
				return this["host"] as string;
			}
		}

		[ConfigurationProperty("cachePort", IsRequired = true)]
		public int CachePort
		{
			get
			{
				var config = this["cachePort"];
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
	}
}