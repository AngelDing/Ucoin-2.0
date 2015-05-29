using System.Runtime.Caching;
using FluentAssertions;
using Xunit;
using System;
using Ucoin.Framework.Cache;

namespace Ucoin.Framework.Test.Caching
{
    public class CacheManagerTest
    {
        private readonly ICacheManager cacheManager;

        public CacheManagerTest()
        {
            cacheManager = CacheHelper.MemoryCache;
        }

        [Fact]
        public void cache_get_and_set_test()
        {
            var cacheKey = new CacheKey("GetTest" + DateTime.Now.Ticks);
            var value = "Get Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            cacheManager.Get(cacheKey, () => { return value; }, cachePolicy);


            var existing = cacheManager.Get(cacheKey.Key, () => { return ""; });
            existing.Should().NotBeNull();
            existing.Should().BeSameAs(value);

        }

        [Fact]
        public void cache_expire_test()
        {
            string key = "ExpireTest" + DateTime.Now.Ticks;
            var tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            var result = cacheManager.Get(cacheKey,
                () =>
                    {
                        return value;
                    },
                cachePolicy);
            result.Should().Be(value);

            // add second value with same tag
            string key2 = "ExpireTest2" + DateTime.Now.Ticks;
            var tags2 = new[] { "a", "c" };
            var cacheKey2 = new CacheKey(key2, tags2);
            var value2 = "Test Value 2 " + DateTime.Now;
            var cachePolicy2 = new CachePolicy();

            var result2 = cacheManager.Get(cacheKey2,
                () =>
                    {
                        return value2;
                    },
                cachePolicy2);
            result2.Should().Be(value2);

            // add third value with same tag
            string key3 = "ExpireTest3" + DateTime.Now.Ticks;
            var tags3 = new[] { "b", "c" };
            var cacheKey3 = new CacheKey(key3, tags3);
            var value3 = "Test Value 3 " + DateTime.Now;
            var cachePolicy3 = new CachePolicy();

            var result3 = cacheManager.Get(cacheKey3,
               () =>
               {
                   return value3;
               },
                cachePolicy3);
            result3.Should().Be(value3);


            var cacheTag = new CacheTag("a");
            string tagKey = new StaticCache().GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            var cachedTag = cacheManager.Get(tagKey, () => { return 0L; });
            cachedTag.Should().NotBe(0L);

            // expire actually just changes the value for tag key
            cacheManager.Expire(cacheTag);

            // allow flush
            System.Threading.Thread.Sleep(500);

            var expiredTag = cacheManager.Get(tagKey, () => { return 0L; });
            expiredTag.Should().NotBe(0L);
            expiredTag.Should().NotBe(cachedTag);

            // items should have been removed
            var expiredValue = cacheManager.Get(cacheKey.Key, () => { return ""; });
            expiredValue.Should().BeEmpty();
            var expiredValue2 = cacheManager.Get(cacheKey2.Key, () => { return ""; });
            expiredValue2.Should().BeEmpty();

            var expiredValue3 = cacheManager.Get(cacheKey3.Key, () => { return ""; });
            expiredValue3.Should().NotBeNull();
        }      

        [Fact]
        public void cache_remove_test()
        {
            var cacheKey = new CacheKey("RemoveTest" + DateTime.Now.Ticks);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            cacheManager.Get(cacheKey, () => { return value; }, cachePolicy);

            // look in underlying MemoryCache
            var cachedValue = cacheManager.Get(cacheKey.Key, () => { return ""; });
            cachedValue.Should().NotBeEmpty();
            cachedValue.Should().Be(value);

            cacheManager.Remove(cacheKey.Key);

            // look in underlying MemoryCache
            var previous = cacheManager.Get(cacheKey.Key, () => { return ""; });
            previous.Should().BeEmpty();
        }
    }
}
