using StackExchange.Redis;
using System;
using System.Configuration;
using Ucoin.Framework.Configurations;

namespace Ucoin.Framework.Redis
{
    public class StackExchangeRedisFactory
    {
        private static ConnectionMultiplexer redisConnection = null;
        private IRedisConfiguration configuration;
        private static readonly object SyncLock = new object();

        public StackExchangeRedisFactory(IRedisConfiguration configuration = null)
        {
            if (configuration == null)
            {
                configuration = RedisConfigurationHandler.GetConfig();
            }

            if (configuration == null)
            {
                throw new ConfigurationErrorsException("Unable to locate <redisConfig> section into your configuration file. Take a look https://github.com/imperugo/StackExchange.Redis.Extensions");
            }

            this.configuration = configuration;
        }

        public IDatabase GetDatabase()
        {
            var connection = ConstructCacheInstance();
            return connection.GetDatabase(configuration.Database);
        }

        public bool IsEndPointReadonly(string hostName)
        {
            foreach (RedisHost host in configuration.RedisHosts)
            {
                if (host.HostFullName == hostName)
                {
                    return host.IsReadonly;
                }
            }

            return false;
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

        public ConfigurationOptions ConstructConnectionOptions()
        {
            var redisOptions = new ConfigurationOptions
            {
                Ssl = configuration.Ssl,
                AllowAdmin = configuration.AllowAdmin,
                ConnectTimeout = configuration.ConnectTimeout,
                KeepAlive = 5,
                DefaultVersion = new Version("2.8.19"),
                Proxy = Proxy.None
            };

            foreach (RedisHost redisHost in configuration.RedisHosts)
            {
                redisOptions.EndPoints.Add(redisHost.IP, redisHost.Port);
            }
            return redisOptions;
        }
    }
}
