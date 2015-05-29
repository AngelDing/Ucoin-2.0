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
        /// <summary>
        /// Encoding to use to convert string to byte[] and the other way around.
        /// </summary>
        /// <remarks>
        /// StackExchange.Redis uses Encoding.UTF8 to convert strings to bytes,
        /// hence we do same here.
        /// </remarks>
        private static readonly Encoding encoding = Encoding.UTF8;

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
            var factory = new RedisCacheFactory(configuration);
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
                var blobBytes = (byte[])data;
                var jsonString = encoding.GetString(blobBytes);
                var deserialisedObject = serializer.Deserialize<T>(jsonString);
                return deserialisedObject;
            }

            return default(T);
        }

        public void Set(CacheKey key, object value, CachePolicy cachePolicy)
        {
            var jsonString = serializer.SerializeToString(value);
            var entryBytes = encoding.GetBytes(jsonString);
            var expiry = ComputeExpiryTimeSpan(cachePolicy);

            db.StringSet(key.Key, entryBytes, expiry);
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
                db.Multiplexer.GetServer(endpoint).FlushDatabase(db.Database);
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
