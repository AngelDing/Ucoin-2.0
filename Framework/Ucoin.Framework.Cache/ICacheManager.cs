using System;
using System.Threading.Tasks;

namespace Ucoin.Framework.Cache
{
    public interface ICacheManager
    {
        //T Get<T>(string key, Func<T> acquirer, int cacheMinutes = 1, bool isAbsoluteExpiration = true);

        T Get<T>(string key, Func<T> acquirer, CachePolicy cachePolicy = null);

        T Get<T>(string key);

        T Get<T>(CacheKey key, Func<T> acquirer, CachePolicy cachePolicy = null);

        //Task<T> GetAsync<T>(CacheKey cacheKey, Func<T> acquirer, CachePolicy cachePolicy);     

        void Set(string key, object value, CachePolicy cachePolicy = null);

        void Set(CacheKey key, object value, CachePolicy cachePolicy = null);
         
        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Expire(string tag);

        void ClearAll();
    }
}
