using Xunit;
using System;
using System.Linq;
using StackExchange.Redis;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.Cache;
using FluentAssertions;

namespace Ucoin.Framework.Test.Caching
{
	public class RedisCacheTest : IDisposable
	{
        private readonly ICacheManager cacheManager;

        public RedisCacheTest()
		{
            cacheManager = CacheHelper.RedisCache;
		}

        [Fact]
        public void redis_cache_set_test()
        {
            cacheManager.Set("my Key", "my value");

            cacheManager.Get<string>("my Key").Should().NotBeEmpty();
        }

        [Fact]
        public void redis_cache_set_complex_test()
        {
            var testobject = new TestClass<DateTime>();
            testobject.Key = "Jacky";
            testobject.Value = DateTime.Now;
            cacheManager.Set("my Key", testobject);
            var result = cacheManager.Get<TestClass<DateTime>>("my Key");

            result.Should().NotBeNull();
            result.Key.Should().Be(testobject.Key);
            //Jil默認將DataTime類型轉為Utc時間，使用時注意同LocalTime的區別
            var newValue = result.Value.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            newValue.Should().Be(testobject.Value.ToString("yyyy/MM/dd HH:mm:ss"));
        }

        [Fact]
        public void redis_cache_remove_test()
        {
            var key = "my Key";
            cacheManager.Set(key, "my value");
            cacheManager.Remove(key);
            cacheManager.Get<string>(key).Should().BeNullOrEmpty();
        }

        [Fact]
        public void redis_cache_remove_by_pattern_test()
        {
            var key1 = "Key:Jakcy:1";
            var key2 = "Key:JakcyX:2";
            var key3 = "Key:JakcyX:3";
            cacheManager.Set(key1, "my value");
            cacheManager.Set(key2, "my value");
            cacheManager.Set(key3, "my value");
            cacheManager.RemoveByPattern("*:Jakcy:*");
            cacheManager.Get<string>(key1).Should().BeNullOrEmpty();
            cacheManager.Get<string>(key2).Should().NotBeNullOrEmpty();
        }

		public void Dispose()
		{
            cacheManager.ClearAll();
		}
	}
}
