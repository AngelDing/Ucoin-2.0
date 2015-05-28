using System.Collections.Generic;
using System.Configuration;
using Ucoin.Framework.Extensions;

namespace Ucoin.Framework.Cache
{
    public class CacheConfig
    {
        public const string AppSettingsKeyPrefix = "Cache.";

        private List<ServerNode> _serverNodes = new List<ServerNode>();
        public List<ServerNode> ServerNodes { get { return _serverNodes; } }

        private Dictionary<string, string> _providerSpecificValues = new Dictionary<string, string>();
        public Dictionary<string, string> ProviderSpecificValues { get { return _providerSpecificValues; } }

        public string CacheSpecificData { get; set; }

        public string DistributedCacheServers { get; set; }

        public CacheConfig()
        {
            InitConfiguration();
        }

        private void InitConfiguration()
        {
            var cacheSpecificDataKey = string.Format("{0}CacheSpecificData", AppSettingsKeyPrefix);
            var distributedCacheServersKey = string.Format("{0}DistributedCacheServers", AppSettingsKeyPrefix);

            if (ConfigurationManager.AppSettings[cacheSpecificDataKey].HasValue())
            {
                CacheSpecificData = ConfigurationManager.AppSettings[cacheSpecificDataKey];
            }

            if (ConfigurationManager.AppSettings[distributedCacheServersKey].HasValue())
            {
                DistributedCacheServers = ConfigurationManager.AppSettings[distributedCacheServersKey];
            }
        }
    }
}
