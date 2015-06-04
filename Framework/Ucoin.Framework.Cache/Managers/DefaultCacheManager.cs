using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ucoin.Framework.Utils;
using Ucoin.Framework.Threading;

namespace Ucoin.Framework.Cache
{
    public class CacheManager<TCache> : ICacheManager where TCache : ICacheProvider
    {
		private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
		private readonly ICacheProvider cacheProvider;

        public CacheManager(Func<Type, ICacheProvider> fn)
        {
            this.cacheProvider = fn(typeof(TCache));
        } 

        public T Get<T>(string key, Func<T> acquirer, CachePolicy cachePolicy = null)
        {
            GuardHelper.ArgumentNotEmpty(() => key);
            var cacheKey = new CacheKey(key);
            return Get(cacheKey, acquirer, cachePolicy);
        }

        public T Get<T>(string key)
        {
            GuardHelper.ArgumentNotEmpty(() => key);
            if (cacheProvider.Contains(key))
            {
                return cacheProvider.Get<T>(key);
            }
            else
            {
                return default(T);
            }
        }

        public T Get<T>(CacheKey cacheKey, Func<T> acquirer, CachePolicy cachePolicy = null)
        {
            var strKey = cacheKey.Key;
            GuardHelper.ArgumentNotEmpty(() => strKey);

            if (cachePolicy == null)
            {
                cachePolicy = new CachePolicy();
            }

            if (cacheProvider.Contains(strKey))
            {
                var value = cacheProvider.Get<T>(strKey);
                if (cacheProvider.CacheType == CacheType.Redis
                    && cachePolicy.ExpirationType == CacheExpirationType.Sliding)
                {
                    this.Set(cacheKey, value, cachePolicy);
                }
                return value;
            }
            else
            {
                if (acquirer == null)
                {
                    return default(T);
                }
                using (cacheProvider.EnterReadLock())
                {
                    if (!cacheProvider.Contains(strKey))
                    {
                        var value = acquirer();
                        this.Set(cacheKey, value, cachePolicy);

                        return value;
                    }
                }

                return cacheProvider.Get<T>(strKey);
            }
        }

        public void Set(string key, object value, CachePolicy cachePolicy = null)
        {
            GuardHelper.ArgumentNotEmpty(() => key);
            var cacheKey = new CacheKey(key);
            Set(cacheKey, value, cachePolicy);
        }

        public void Set(CacheKey cacheKey, object value, CachePolicy cachePolicy = null)
        {
            var strKey = cacheKey.Key;
            GuardHelper.ArgumentNotEmpty(() => strKey);

            if (value == null)
            {
                return;
            }

            if (cachePolicy == null)
            {
                cachePolicy = new CachePolicy();
            }

            using (cacheProvider.EnterWriteLock())
            {
                cacheProvider.Set(cacheKey, value, cachePolicy);
            }
        }

        public void Remove(string key)
        {
            GuardHelper.ArgumentNotEmpty(() => key);

            using (cacheProvider.EnterWriteLock())
			{
				cacheProvider.Remove(key);
			}
        }

        public void RemoveByPattern(string pattern)
        {
            cacheProvider.RemoveByPattern(pattern);
        }

        public void ClearAll()
        {
            cacheProvider.ClearAll();
        }

        public void Expire(string tag)
        {
            var cacheTag = new CacheTag(tag);
            cacheProvider.Expire(cacheTag);
        }


        public CacheType CacheType
        {
            get { return cacheProvider.CacheType; }
        }
    }
}