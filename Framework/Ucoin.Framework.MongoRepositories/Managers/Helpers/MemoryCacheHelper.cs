using System;
using System.Runtime.Caching;

namespace Ucoin.Framework.MongoDb.Managers
{
    public class MemoryCacheHelper
    {
        private static readonly Object _locker = new object();

        public static T GetCacheItem<T>(string key, Func<T> cachePopulate,
            TimeSpan? slidingExpiration = null, DateTime? absoluteExpiration = null)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Invalid cache key");
            }
            if (cachePopulate == null)
            {
                throw new ArgumentNullException("cachePopulate");
            }
            if (slidingExpiration == null && absoluteExpiration == null)
            {
                throw new ArgumentException("Either a sliding expiration or absolute must be provided");
            }

            if (MemoryCache.Default[key] == null)
            {
                lock (_locker)
                {
                    if (MemoryCache.Default[key] == null)
                    {
                        var item = new CacheItem(key, cachePopulate());
                        var policy = CreatePolicy(slidingExpiration, absoluteExpiration);

                        MemoryCache.Default.Add(item, policy);
                    }
                }
            }

            return (T)MemoryCache.Default[key];
        }

        public static void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public static object Get(string key)
        {
            return MemoryCache.Default[key];
        }

        private static CacheItemPolicy CreatePolicy(TimeSpan? slidingExpiration, DateTime? absoluteExpiration)
        {
            var policy = new CacheItemPolicy();

            if (absoluteExpiration.HasValue)
            {
                policy.AbsoluteExpiration = absoluteExpiration.Value;
            }
            else if (slidingExpiration.HasValue)
            {
                policy.SlidingExpiration = slidingExpiration.Value;
            }

            policy.Priority = CacheItemPriority.Default;

            return policy;
        }
    }
}
