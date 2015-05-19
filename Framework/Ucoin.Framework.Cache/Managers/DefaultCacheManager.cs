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
            if (cachePolicy == null)
            {
                cachePolicy = new CachePolicy();
            }
            return Get(cacheKey, acquirer, cachePolicy);
        }

        public T Get<T>(string key)
        {
            GuardHelper.ArgumentNotEmpty(() => key);
            if (cacheProvider.Contains(key))
            {
                return (T)cacheProvider.Get(key);
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
                return (T)cacheProvider.Get(strKey);
            }
            else
            {
                if (acquirer == null)
                {
                    return default(T);
                }
                using (EnterReadLock())
                {
                    if (!cacheProvider.Contains(strKey))
                    {
                        var value = acquirer();
                        this.Set(cacheKey, value, cachePolicy);

                        return value;
                    }
                }

                return (T)cacheProvider.Get(strKey);
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

            using (EnterWriteLock())
            {
                cacheProvider.Set(cacheKey, value, cachePolicy);
            }
        }

        public void Remove(string key)
        {
            GuardHelper.ArgumentNotEmpty(() => key);

			using (EnterWriteLock())
			{
				cacheProvider.Remove(key);
			}
        }

        public void RemoveByPattern(string pattern)
        {
            GuardHelper.ArgumentNotEmpty(() => pattern);
			
			var regex = new Regex(pattern, RegexOptions.Singleline 
                | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in cacheProvider.Entries)
            {
                if (regex.IsMatch(item.Key))
                {
                    keysToRemove.Add(item.Key);
                }
            }

			using (EnterWriteLock())
			{
				foreach (string key in keysToRemove)
				{
					cacheProvider.Remove(key);
				}
			}
        }

        public void Clear()
        {
            var keysToRemove = new List<string>();
            foreach (var item in cacheProvider.Entries)
            {
                keysToRemove.Add(item.Key);
            }

			using (EnterWriteLock())
			{
				foreach (string key in keysToRemove)
				{
					cacheProvider.Remove(key);
				}
			}
        }

		private IDisposable EnterReadLock()
		{
            return cacheProvider.IsSingleton ? rwLock.GetUpgradeableReadLock() : ActionDisposable.Empty;
		}

		public IDisposable EnterWriteLock()
		{
			return cacheProvider.IsSingleton ? rwLock.GetWriteLock() : ActionDisposable.Empty;
		}       
    }
}