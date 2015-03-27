using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Ucoin.Framework.Cache
{    
    public partial class StaticCache : BaseCache, ICacheProvider
    {       
		private ObjectCache _cache;

        protected ObjectCache Cache
        {
            get
            {
				if (_cache == null)
				{
					_cache = new MemoryCache("GroupTour");
				}
				return _cache;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> Entries
        {
            get
            {
				return Cache;
            }
        }

		public object Get(string key)
        {
			return Cache.Get(key);
        }

        public void Set(CacheKey cacheKey, object value, CachePolicy cachePolicy)
        {
            string strKey = GetKey(cacheKey);
            var cacheItem = new CacheItem(strKey, value);
            var policy = CreatePolicy(cacheKey, cachePolicy);

            Cache.Add(cacheItem, policy);
        }

        //public async Task SetAsync(CacheKey cacheKey, object value, CachePolicy cachePolicy)
        //{
        //    await Set(cacheKey, value, cachePolicy);
        //}

        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

		public bool IsSingleton
		{
			get { return true; }
		}        

        internal CacheItemPolicy CreatePolicy(CacheKey key, CachePolicy cachePolicy)
        {
            var policy = new CacheItemPolicy();

            switch (cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Sliding:
                    policy.SlidingExpiration = cachePolicy.SlidingExpiration;
                    break;
                case CacheExpirationType.Absolute:
                    policy.AbsoluteExpiration = cachePolicy.AbsoluteExpiration;
                    break;
                case CacheExpirationType.Duration:
                    policy.AbsoluteExpiration = DateTimeOffset.Now.Add(cachePolicy.Duration);
                    break;
                default:
                    policy.AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
                    break;
            }

            var changeMonitor = CreateChangeMonitor(key);
            if (changeMonitor != null)
            {
                policy.ChangeMonitors.Add(changeMonitor);
            }

            return policy;
        }

        internal CacheEntryChangeMonitor CreateChangeMonitor(CacheKey key)
        {
            var tags = GetTagKeyList(key);

            if (tags.Count == 0)
            {
                return null;
            }

            var absoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration;
            var value = DateTimeOffset.UtcNow.Ticks;
            foreach (string tag in tags)
            {
                Cache.Add(tag, value, absoluteExpiration);
            }

            return Cache.CreateCacheEntryChangeMonitor(tags);
        }
    }
}
