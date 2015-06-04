using Xunit;
using System;
using System.Linq;
using StackExchange.Redis;
using Ucoin.Framework.Serialization;
using Ucoin.Framework.Cache;
using FluentAssertions;

namespace Ucoin.Framework.Test.Caching
{
	public class RedisCacheTest : CacheManagerTest
	{
        public RedisCacheTest()
            : base(CacheHelper.RedisCache)
		{
		}

        [Fact]
        public void cache_redis_constructor_test()
        {
            Action action = () => new RedisCache();
            action.ShouldNotThrow();
        }

        [Fact]
        public void cache_redis_set_complex_test()
        {
            var testobject = new TestClass<DateTime>();
            testobject.Key = "Jacky";
            testobject.Value = DateTime.Now;
            CacheManager.Set("my Key", testobject);
            var result = CacheManager.Get<TestClass<DateTime>>("my Key");

            result.Should().NotBeNull();
            result.Key.Should().Be(testobject.Key);
            //Jil默認將DataTime類型轉為Utc時間，使用時注意同LocalTime的區別
            var newValue = result.Value.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            newValue.Should().Be(testobject.Value.ToString("yyyy/MM/dd HH:mm:ss"));
        }

        [Fact]
        public void cache_redis_remove_by_pattern_test()
        {
            var key1 = "Key:Jakcy:1";
            var key2 = "Key:JakcyX:2";
            var key3 = "Key:JakcyX:3";
            CacheManager.Set(key1, "my value");
            CacheManager.Set(key2, "my value");
            CacheManager.Set(key3, "my value");
            CacheManager.RemoveByPattern("*:Jakcy:*");
            CacheManager.Get<string>(key1).Should().BeNullOrEmpty();
            CacheManager.Get<string>(key2).Should().NotBeNullOrEmpty();
        }
	}
}
