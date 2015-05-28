﻿using StackExchange.Redis;
using System;

namespace Ucoin.Framework.Cache
{
    public class RedisCacheFactory : DistributedCacheFactory
    {
        private ConnectionMultiplexer _redisConnection = null;
        private const string DEFAULT_IpAddress = "127.0.0.1";
        private const int DEFAULT_Port = 6379;

        public RedisCacheFactory(CacheConfig config = null)
            : base(config)
        {
            ParseConfig(DEFAULT_IpAddress, DEFAULT_Port);
        }

        internal ConnectionMultiplexer ConstructCacheInstance()
        {
            var connectionOptions = ConstructConnectionOptions();

            try
            {
                _redisConnection = ConnectionMultiplexer.Connect(connectionOptions);
            }
            catch (Exception ex)
            {
                //Logger.WriteException(ex);
                throw ex;
            }

            return _redisConnection;
        }

        private ConfigurationOptions ConstructConnectionOptions()
        {
            var redisOptions = new ConfigurationOptions();
            if (!string.IsNullOrWhiteSpace(CacheConfiguration.CacheSpecificData))
            {
                // Note: The redis config parser requires values be separated by a comma, not a semi-colon which is what CacheAdapter likes
                var redisSpecificOptions = CacheConfiguration.CacheSpecificData.Replace(';', ',');
                redisOptions = ConfigurationOptions.Parse(redisSpecificOptions, true);
            }

            // Clear the endpoints if any specified here as we use the ones defined in DistributedCacheServers setting to keep
            // config consistent and it means that users can switch to different cache providers without issues
            redisOptions.EndPoints.Clear();
            CacheConfiguration.ServerNodes.ForEach(n =>
            {
                redisOptions.EndPoints.Add(n.IPAddressOrHostName, n.Port);
            });
            return redisOptions;
        }
    }
}