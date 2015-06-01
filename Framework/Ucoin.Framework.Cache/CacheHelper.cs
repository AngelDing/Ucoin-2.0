
using Ucoin.Framework.Dependency;
namespace Ucoin.Framework.Cache
{
    public static class CacheHelper
    {
        public static ICacheManager WebCache
        {
            get
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<AspNetCache>>();
            }
        }

        public static ICacheManager MemoryCache
        {
            get
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<StaticCache>>();
            }
        }

        public static ICacheManager RedisCache
        {
            get
            {
                return SimpleLocator<CacheLocator>.Current.Resolve<CacheManager<RedisCache>>();
            }
        }
    }
}
