using System;
using Ucoin.Framework.Cache;
using Xunit;
using FluentAssertions;

namespace Ucoin.Framework.Test.Caching
{
    public class WebCacheTest  : CacheManagerTest
	{
        public WebCacheTest()
            : base(CacheHelper.WebCache)
		{
		}

        [Fact]
        public void cache_web_constructor_test()
        {
            Action action = () => new StaticCache();
            action.ShouldNotThrow();
        }

        [Fact]
        public void cache_memary_remove_by_pattern_test()
        {
            var key1 = "Key:Jakcy:1";
            var key2 = "Key:JakcyX:2";
            var key3 = "Key:JakcyX:3";
            CacheManager.Set(key1, "my value");
            CacheManager.Set(key2, "my value");
            CacheManager.Set(key3, "my value");
            CacheManager.RemoveByPattern(":Jakcy:");
            CacheManager.Get<string>(key1).Should().BeNullOrEmpty();
            CacheManager.Get<string>(key2).Should().NotBeNullOrEmpty();
        }
    }
}
