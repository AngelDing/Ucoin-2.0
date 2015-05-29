using StackExchange.Redis;
using System;
using System.Configuration;

namespace Ucoin.Framework.Cache
{
    public class RedisCacheFactory
    {
        private ConnectionMultiplexer redisConnection = null;
        private IRedisCachingConfiguration configuration;

        public RedisCacheFactory(IRedisCachingConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = RedisCachingSectionHandler.GetConfig();
            }

            if (configuration == null)
            {
                throw new ConfigurationErrorsException("Unable to locate <redisCacheClient> section into your configuration file. Take a look https://github.com/imperugo/StackExchange.Redis.Extensions");
            }
        }

        internal ConnectionMultiplexer ConstructCacheInstance()
        {
            var connectionOptions = ConstructConnectionOptions();

            try
            {
                redisConnection = ConnectionMultiplexer.Connect(connectionOptions);
            }
            catch (Exception ex)
            {
                //Logger.WriteException(ex);
                throw ex;
            }

            return redisConnection;
        }

        private ConfigurationOptions ConstructConnectionOptions()
        {
            var redisOptions = new ConfigurationOptions
            {
                Ssl = configuration.Ssl,
                AllowAdmin = configuration.AllowAdmin
            };

            foreach (RedisHost redisHost in configuration.RedisHosts)
            {
                redisOptions.EndPoints.Add(redisHost.Host, redisHost.CachePort);
            }
            return redisOptions;
        }
    }
}
