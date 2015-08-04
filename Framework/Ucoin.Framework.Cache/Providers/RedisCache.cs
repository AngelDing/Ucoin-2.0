using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Ucoin.Framework.Serialization;
using System.Configuration;
using Ucoin.Framework.Extensions;
using Ucoin.Framework.Configurations;
using Ucoin.Framework.Redis;
using Ucoin.Framework.Utils;

namespace Ucoin.Framework.Cache
{
    public class RedisCache : BaseCache, ICacheProvider
    {
        private readonly ISerializer serializer;
        private readonly IRedisWrapper redisWrapper;

        public RedisCache()
            : this(SerializationHelper.Jil)
        { 
        }

        public RedisCache(ISerializer serializer, IRedisWrapper redisWrapper = null)
        {
            GuardHelper.ArgumentNotNull(() => serializer);
            if (redisWrapper == null)
            {
                redisWrapper = RedisWrapperFactory.GetRedisWrapper();
            }

            this.serializer = serializer;
            this.redisWrapper = redisWrapper;
        }

        public CacheType CacheType
        {
            get { return CacheType.Redis; }
        }

        public T Get<T>(string key)
        {
            var data = redisWrapper.Get(key); 
            if (data != null)
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

            redisWrapper.Set(key.Key, jsonString, expiry);

            ManageCacheDependencies(key);
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
                    expiry = cachePolicy.AbsoluteExpiration - DateTimeOffset.UtcNow;
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
            return redisWrapper.Exists(key);
        }

        public void Remove(string key)
        {
            redisWrapper.Remove(key);
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
            redisWrapper.RemoveByPattern(pattern);
        }

        public void ClearAll()
        {
            redisWrapper.ClearAll();
        }

        public void Expire(CacheTag cacheTag)
        {
            string key = GetTagKey(cacheTag);
            var childKeys = this.Get<List<string>>(key);
            if (childKeys.IsNullOrEmpty() == false)
            {
                childKeys.ForEach(k => Remove(k));
            }
            Remove(key);
        }

        public override bool IsSingleton()
        {
            return false;
        }

        private void ManageCacheDependencies(CacheKey cacheKey)
        {
            if (cacheKey.Tags.Any())
            {
                foreach (var tag in cacheKey.Tags)
                {
                    var tagStr = GetTagKey(tag);
                    var childKeys = this.Get<List<string>>(tagStr);
                    if (childKeys.IsNullOrEmpty() == true)
                    {
                        childKeys = new List<string>();
                    }
                    var childKey = cacheKey.Key;
                    if (childKeys.Contains(childKey) == false)
                    {
                        childKeys.Add(childKey);
                    }
                    var tagKey = new CacheKey(tagStr);
                    var cachePolicy = CachePolicy.WithAbsoluteExpiration(DateTimeOffset.UtcNow.AddYears(10));
                    this.Set(tagKey, childKeys, cachePolicy);
                }
            }
        }
    }
}