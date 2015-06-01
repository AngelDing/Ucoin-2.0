using System.ComponentModel;

namespace Ucoin.Framework.Cache
{
    public enum CacheType
    {
        [Description("Web")]
        Web = 1,
        [Description("Memory")]
        Memory = 2,
        [Description("Redis")]
        Redis = 3,

        //AppFabric,

        //Memcached,

        //AzureTableStorage,

        //Disk
    }
}
