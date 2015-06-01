using StackExchange.Redis;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ucoin.Framework.Serialization;
using System.Configuration;

namespace Ucoin.Framework.Cache
{
    /// <summary>
    /// 暫不支持相對過期策略，使用redis緩存，只能指定過期時間；
    /// 如果要支持，則需要將緩存內容再包裝一層，將過期策略也緩存，然後取得的時候，根據策略，判斷是否需要重新指定到期日期
    /// </summary>
    public class RedisCache : BaseCache, ICacheProvider
    {
        private static IDatabase db = null;
        private readonly ISerializer serializer;
        private readonly RedisCacheFactory factory;

        public RedisCache()
            : this(Serializer.Jil)
        { 
        }

        public RedisCache(ISerializer serializer, IRedisCachingConfiguration configuration = null)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            this.serializer = serializer;
            factory = new RedisCacheFactory(configuration);
            db = factory.GetDatabase();
            this.serializer = serializer;
        }

        public CacheType CacheType
        {
            get { return CacheType.Redis; }
        }

        public T Get<T>(string key)
        {
            var data = db.StringGet(key);
            if (!data.IsNull && data.HasValue)
            {
                var deserialisedObject = serializer.Deserialize<T>(data);
                return deserialisedObject;
            }

            return default(T);
        }

        public void Set(CacheKey key, object value, CachePolicy cachePolicy)
        {
            var jsonString = serializer.SerializeToString(value);
            var expiry = ComputeExpiryTimeSpan(cachePolicy);

            db.StringSet(key.Key, jsonString, expiry);
        }

        private TimeSpan? ComputeExpiryTimeSpan(CachePolicy cachePolicy)
        {
            TimeSpan? expiry = null;
            switch (cachePolicy.ExpirationType)
            {
                case CacheExpirationType.Sliding:
                    expiry = cachePolicy.SlidingExpiration;
                    break;
                case CacheExpirationType.Absolute:
                    expiry = cachePolicy.AbsoluteExpiration - DateTimeOffset.Now;
                    break;
                case CacheExpirationType.Duration:
                    expiry = cachePolicy.Duration;
                    break;
                default:
                    break;
            }

            return expiry;
        }

        public bool Contains(string key)
        {
            return db.KeyExists(key);
        }

        public void Remove(string key)
        {
            db.KeyDelete(key);
        }

        /// <summary>
        /// 批量刪除緩存
        /// </summary>
        /// <param name="pattern">模式匹配</param>
        /// <example>
        /// if you want to return all keys that start with "myCacheKey" uses "myCacheKey*"
        ///	if you want to return all keys that contain with "myCacheKey" uses "*myCacheKey*"
        ///	if you want to return all keys that end with "myCacheKey" uses "*myCacheKey"
        /// </example>
        public void RemoveByPattern(string pattern)
        {
            var keys = new List<RedisKey>();

            var endPoints = db.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                var dbKeys = db.Multiplexer.GetServer(endpoint).Keys(pattern: pattern);

                foreach (var dbKey in dbKeys)
                {
                    if (!keys.Contains(dbKey))
                    {
                        keys.Add(dbKey);
                    }
                }
            }

            keys.ForEach(k => Remove(k));
        }

        public void ClearAll()
        {
            var endPoints = db.Multiplexer.GetEndPoints();

            foreach (var endpoint in endPoints)
            {
                if (factory.IsEndPointReadonly(endpoint.ToString()) == false)
                {
                    db.Multiplexer.GetServer(endpoint).FlushDatabase(db.Database);
                }
            }
        }

        public void Expire(CacheTag cacheTag)
        {
            string key = GetTagKey(cacheTag);
            db.KeyDelete(key);
        }

        public override bool IsSingleton()
        {
            return false;
        }
    }
}
