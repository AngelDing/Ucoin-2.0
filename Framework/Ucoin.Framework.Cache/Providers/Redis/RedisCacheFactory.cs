using StackExchange.Redis;
using System;
using System.Configuration;

namespace Ucoin.Framework.Cache
{
    public class RedisCacheFactory
    {
        private static ConnectionMultiplexer redisConnection = null;
        private IRedisCachingConfiguration configuration;
        private static readonly object SyncLock = new object();

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

            this.configuration = configuration;
        }

        internal IDatabase GetDatabase()
        {
            var connection = ConstructCacheInstance();
            return connection.GetDatabase(configuration.Database);
        }

        private ConnectionMultiplexer ConstructCacheInstance()
        {
            if (redisConnection == null || !redisConnection.IsConnected 
                || !redisConnection.GetDatabase().IsConnected(default(RedisKey)))
            {
                lock (SyncLock)
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
                }
            }

            return redisConnection;
        }

        private ConfigurationOptions ConstructConnectionOptions()
        {
            var redisOptions = new ConfigurationOptions
            {
                Ssl = configuration.Ssl,
                AllowAdmin = configuration.AllowAdmin,
                ConnectTimeout = configuration.ConnectTimeout
            };

            foreach (RedisHost redisHost in configuration.RedisHosts)
            {
                redisOptions.EndPoints.Add(redisHost.IP, redisHost.Port);
            }
            return redisOptions;
        }       
    }
}
