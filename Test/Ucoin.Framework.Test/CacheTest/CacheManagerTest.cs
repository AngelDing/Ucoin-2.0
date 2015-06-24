using System.Runtime.Caching;
using FluentAssertions;
using Xunit;
using System;
using Ucoin.Framework.Cache;
using System.Globalization;

namespace Ucoin.Framework.Test.Caching
{
    public class CacheManagerTest : DisposableObject
    {
        public ICacheManager CacheManager { get; private set; }

        public CacheManagerTest()
            : this(CacheHelper.WebCache)
        {
        }

        public CacheManagerTest(ICacheManager cacheManager)
        {
            if (cacheManager == null)
            {
                throw new ArgumentNullException("cacheManager");
            }
            this.CacheManager = cacheManager;
        }

        [Fact]
        public void cache_get_and_set_test()
        {
            var cacheKey = new CacheKey("GetTest" + DateTime.Now.Ticks);
            var value = "Get Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            CacheManager.Get(cacheKey, () => { return value; }, cachePolicy);

            var existing = CacheManager.Get<string>(cacheKey.Key, () => { return ""; });
            existing.Should().NotBeNull();
            existing.Should().Be(value);
        }           

        [Fact]
        public void cache_remove_test()
        {
            var cacheKey = new CacheKey("RemoveTest" + DateTime.Now.Ticks);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            CacheManager.Get(cacheKey, () => { return value; }, cachePolicy);

            // look in underlying MemoryCache
            var cachedValue = CacheManager.Get(cacheKey.Key, () => { return ""; });
            cachedValue.Should().NotBeEmpty();
            cachedValue.Should().Be(value);

            CacheManager.Remove(cacheKey.Key);

            // look in underlying MemoryCache
            var previous = CacheManager.Get(cacheKey.Key, () => { return ""; });
            previous.Should().BeEmpty();
        }        

        [Fact]
        public void cache_expire_test()
        {
            string key = "ExpireTest" + DateTime.Now.Ticks;
            var tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            var result = CacheManager.Get<string>(cacheKey,
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

            var result2 = CacheManager.Get<string>(cacheKey2,
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

            var result3 = CacheManager.Get<string>(cacheKey3,
               () =>
               {
                   return value3;
               },
                cachePolicy3);
            result3.Should().Be(value3);


            var cacheTag = new CacheTag("a");
            string tagKey = new StaticCache().GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            var cachedTag = CacheManager.Get<object>(tagKey, () => { return null; });
            if (CacheManager.CacheType == CacheType.Web)
            {
                cachedTag.Should().BeNull();
            }
            else
            {
                cachedTag.Should().NotBeNull();
            }
            // expire actually just changes the value for tag key
            CacheManager.Expire(cacheTag);

            // allow flush
            System.Threading.Thread.Sleep(500);

            var expiredTag = CacheManager.Get<object>(tagKey, () => { return null; });
            expiredTag.Should().BeNull();

            // items should have been removed
            var expiredValue = CacheManager.Get<string>(cacheKey.Key, () => { return ""; });
            expiredValue.Should().BeEmpty();
            var expiredValue2 = CacheManager.Get<string>(cacheKey2.Key, () => { return ""; });
            expiredValue2.Should().BeEmpty();

            var expiredValue3 = CacheManager.Get<string>(cacheKey3.Key, () => { return ""; });
            expiredValue3.Should().NotBeNull();
        }

        [Fact]
        public void cache_set_test()
        {
            var cacheKey = new CacheKey("AddTest" + DateTime.Now.Ticks);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            CacheManager.Set(cacheKey, value, cachePolicy);

            // look in underlying MemoryCache
            var cachedValue = CacheManager.Get<string>(cacheKey.Key);
            cachedValue.Should().NotBeNull();
            cachedValue.Should().Be(value);
        }

        [Fact]
        public void cache_absolute_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var cacheKey = new CacheKey(key);
            var absoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(5);
            var cachePolicy = CachePolicy.WithAbsoluteExpiration(absoluteExpiration);
            var value = "Jacky zhou";
            this.CacheManager.Get<string>(cacheKey, () => { return value; }, cachePolicy);
            var expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; });
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(5000);

            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; });
            expectValue.Should().Be("");
        }

        [Fact]
        public void cache_sliding_test()
        {            
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var cacheKey = new CacheKey(key);
            var slidingExpiration = TimeSpan.FromSeconds(3);
            var cachePolicy = CachePolicy.WithSlidingExpiration(slidingExpiration);
            var value = "Jacky zhou";

            this.CacheManager.Get<string>(cacheKey, () => { return value; }, cachePolicy);

            System.Threading.Thread.Sleep(1000);
            var expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; }, cachePolicy);
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(2000);
            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; }, cachePolicy);
            expectValue.Should().Be(value);

            System.Threading.Thread.Sleep(3001);
            expectValue = this.CacheManager.Get<string>(cacheKey, () => { return ""; });
            expectValue.Should().Be("");
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                CacheManager.ClearAll();
            }
        }
    }
}
