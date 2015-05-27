using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Caching;
using FluentAssertions;
using Xunit;
using Ucoin.Framework.Cache;

namespace Ucoin.Framework.Test.Caching
{
    public class StaticCacheTest
    {
        private readonly StaticCache provider;
        public StaticCacheTest()
        {
            provider = new StaticCache();
        }

        [Fact]
        public void cache_static_constructor_test()
        {
            Action action = () => new StaticCache();
            action.ShouldNotThrow();
        }

        [Fact]
        public void cache_static_set_test()
        {
            var cacheKey = new CacheKey("AddTest" + DateTime.Now.Ticks);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            provider.Set(cacheKey, value, cachePolicy);

            // look in underlying MemoryCache
            var cachedValue = provider.Get(cacheKey.Key).ToString();
            cachedValue.Should().NotBeNull();
            cachedValue.Should().Be(value);
        }

        [Fact]
        public void cache_static_set_with_tags_test()
        {
            string key = "AddWithTagsTest" + DateTime.Now.Ticks;
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            provider.Set(cacheKey, value, cachePolicy);
    
            // look in underlying MemoryCache
            string innerKey = provider.GetKey(cacheKey);
            var cachedValue = provider.Get(innerKey);
            cachedValue.Should().NotBeNull();
            cachedValue.Should().Be(value);

            // make sure cache key is in underlying MemoryCache
            var cacheTag = new CacheTag("a");
            string tagKey = provider.GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            var cachedTag = provider.Get(tagKey);
            cachedTag.Should().NotBeNull();
        }

        [Fact]
        public void cache_static_set_with_existing_tag_test()
        {
            string key = "AddWithExistingTagTest" + DateTime.Now.Ticks;
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            provider.Set(cacheKey, value, cachePolicy);

            // make sure cache key is in underlying MemoryCache
            var cacheTag = new CacheTag("a");
            string tagKey = provider.GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            var cachedTag = provider.Get(tagKey);
            cachedTag.Should().NotBeNull();

            // add second value with same tag
            string key2 = "AddWithExistingTagTest2" + DateTime.Now.Ticks;
            string[] tags2 = new[] { "a", "c" };
            var cacheKey2 = new CacheKey(key2, tags2);
            var value2 = "Test Value 2 " + DateTime.Now;
            var cachePolicy2 = new CachePolicy();

            provider.Set(cacheKey2, value2, cachePolicy2);

            // tag 'a' should have same value
            var cachedTag2 = provider.Get(tagKey);
            cachedTag2.Should().NotBeNull();
            cachedTag2.Should().Be(cachedTag);
        }

        [Fact]
        public void cache_static_expire_test()
        {
            string key = "ExpireTest" + DateTime.Now.Ticks;
            var tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            provider.Set(cacheKey, value, cachePolicy);

            // add second value with same tag
            string key2 = "ExpireTest2" + DateTime.Now.Ticks;
            var tags2 = new[] { "a", "c" };
            var cacheKey2 = new CacheKey(key2, tags2);
            var value2 = "Test Value 2 " + DateTime.Now;
            var cachePolicy2 = new CachePolicy();

            provider.Set(cacheKey2, value2, cachePolicy2);

            // add third value with same tag
            string key3 = "ExpireTest3" + DateTime.Now.Ticks;
            var tags3 = new[] { "b", "c" };
            var cacheKey3 = new CacheKey(key3, tags3);
            var value3 = "Test Value 3 " + DateTime.Now;
            var cachePolicy3 = new CachePolicy();

            provider.Set(cacheKey3, value3, cachePolicy3);

            var cacheTag = new CacheTag("a");
            string tagKey = provider.GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            // underlying cache
            (provider as StaticCache).GetAllEntries().ToList().Count.Should().Be(6);

            var cachedTag = provider.Get(tagKey);
            cachedTag.Should().NotBeNull();

            System.Threading.Thread.Sleep(500);

            // expire actually just changes the value for tag key
            provider.Expire(cacheTag);

            var expiredTag = provider.Get(tagKey);
            expiredTag.Should().NotBeNull();
            expiredTag.Should().NotBe(cachedTag);

            // items should have been removed
            var expiredValue = provider.Get(cacheKey.Key);
            expiredValue.Should().BeNull();

            var expiredValue2 = provider.Get(cacheKey2.Key);
            expiredValue2.Should().BeNull();

            var expiredValue3 = provider.Get(cacheKey3.Key);
            expiredValue3.Should().NotBeNull();

            (provider as StaticCache).GetAllEntries().ToList().Count.Should().Be(4);
        }

        [Fact]
        public void cache_static_get_test()
        {
            var cacheKey = new CacheKey("GetTest" + DateTime.Now.Ticks);
            var value = "Get Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            provider.Set(cacheKey, value, cachePolicy);

            var existing = provider.Get(cacheKey.Key);
            existing.Should().NotBeNull();
            existing.Should().BeSameAs(value);
        }

        [Fact]
        public void cache_static_remove_test()
        {
            var cacheKey = new CacheKey("RemoveTest" + DateTime.Now.Ticks);
            var value = "Test Value " + DateTime.Now;
            var cachePolicy = new CachePolicy();

            provider.Set(cacheKey, value, cachePolicy);

            // look in underlying MemoryCache
            string innerKey = provider.GetKey(cacheKey);
            var cachedValue = provider.Get(innerKey);
            cachedValue.Should().NotBeNull();
            cachedValue.Should().Be(value);

            provider.Remove(cacheKey.Key);

            // look in underlying MemoryCache
            var previous = provider.Get(innerKey);
            previous.Should().BeNull();
        }

        [Fact]
        public void cache_static_create_change_monitor_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            cacheKey.Should().NotBeNull();

            var monitor = provider.CreateChangeMonitor(cacheKey);
            monitor.Should().NotBeNull();
            monitor.CacheKeys.Should().HaveCount(2);

            var cacheTag = new CacheTag("a");
            string tagKey = provider.GetTagKey(cacheTag);
            tagKey.Should().NotBeNullOrEmpty();

            monitor.CacheKeys.Should().Contain(tagKey);
        }

        [Fact]
        public void cache_static_create_policy_absolute_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            cacheKey.Should().NotBeNull();

            var absoluteExpiration = DateTimeOffset.Now.AddMinutes(5);
            var cachePolicy = CachePolicy.WithAbsoluteExpiration(absoluteExpiration);
            cachePolicy.Should().NotBeNull();

            var policy = provider.CreatePolicy(cacheKey, cachePolicy);
            policy.Should().NotBeNull();
            policy.AbsoluteExpiration.Should().Be(absoluteExpiration);
            policy.ChangeMonitors.Should().NotBeNull();
            policy.ChangeMonitors.Should().HaveCount(1);
            policy.ChangeMonitors.Should().ContainItemsAssignableTo<CacheEntryChangeMonitor>();
        }

        [Fact]
        public void cache_static_create_policy_sliding_test()
        {
            string key = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            string[] tags = new[] { "a", "b" };
            var cacheKey = new CacheKey(key, tags);
            cacheKey.Should().NotBeNull();

            var slidingExpiration = TimeSpan.FromMinutes(5);
            var cachePolicy = CachePolicy.WithSlidingExpiration(slidingExpiration);
            cachePolicy.Should().NotBeNull();

            var policy = provider.CreatePolicy(cacheKey, cachePolicy);
            policy.Should().NotBeNull();
            policy.SlidingExpiration.Should().Be(slidingExpiration);
            policy.ChangeMonitors.Should().NotBeNull();
            policy.ChangeMonitors.Should().HaveCount(1);
            policy.ChangeMonitors.Should().ContainItemsAssignableTo<CacheEntryChangeMonitor>();
        }

    }
}
