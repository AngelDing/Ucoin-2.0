using System;
using Ucoin.Framework.Threading;

namespace Ucoin.Framework.Cache
{
    public partial class NullCacheManager : ICacheManager
    {
		private readonly static ICacheManager s_instance = new NullCacheManager();

		public static ICacheManager Instance
		{
			get { return s_instance; }
		}

        public T Get<T>(string key, Func<T> acquirer, CachePolicy cachePolicy)
        {
            return this.Get(new CacheKey(key), acquirer, cachePolicy);
        }

        public T Get<T>(CacheKey key, Func<T> acquirer, CachePolicy cachePolicy)
        {
            if (acquirer == null)
            {
                return default(T);
            }
            return acquirer();
        }

        public void Remove(string key)
        {
        }

        public void RemoveByPattern(string pattern)
        {
        }

        public void ClearAll()
        {
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public void Set(string key, object value, CachePolicy cachePolicy = null)
        {
        }

        public void Set(CacheKey key, object value, CachePolicy cachePolicy = null)
        {
        }

        public void Expire(string tag)
        {
        }
    }
}