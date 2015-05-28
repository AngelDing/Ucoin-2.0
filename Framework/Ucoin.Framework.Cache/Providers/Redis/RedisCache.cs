using System;

namespace Ucoin.Framework.Cache
{
    public class RedisCache : BaseCache, ICacheProvider
    {
        public CacheType CacheType
        {
            get { return CacheType.Redis; }
        }

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(CacheKey key, object value, CachePolicy cachePolicy)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }

        public void Expire(CacheTag cacheTag)
        {
            throw new NotImplementedException();
        }

        public override bool IsSingleton()
        {
            return false;
        }
    }
}
