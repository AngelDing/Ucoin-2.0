//using System.Runtime.Caching;
//using FluentAssertions;
//using Xunit;
//using System;
//using Ucoin.Framework.Cache;

//namespace Ucoin.Framework.Test
//{    
//    public class CachePolicyTest
//    {
//        [Fact]
//        public void CachePolicyConstructorTest()
//        {
//            var cachePolicy = new CachePolicy();
            
//            cachePolicy.Should().NotBeNull();
//            cachePolicy.ExpirationType.Should().Be(CacheExpirationType.None);
//            cachePolicy.AbsoluteExpiration.Should().Be(ObjectCache.InfiniteAbsoluteExpiration);
//            cachePolicy.SlidingExpiration.Should().Be(ObjectCache.NoSlidingExpiration);
//        }

//        [Fact]
//        public void WithAbsoluteExpirationTest()
//        {
//            var absoluteExpiration = new DateTimeOffset(2012, 1, 1, 12, 0, 0, TimeSpan.Zero);
//            var cachePolicy = CachePolicy.WithAbsoluteExpiration(absoluteExpiration);
            
//            cachePolicy.Should().NotBeNull();
//            cachePolicy.Mode.Should().Be(CacheExpirationType.Absolute);
//            cachePolicy.AbsoluteExpiration.Should().Be(absoluteExpiration);
//            cachePolicy.SlidingExpiration.Should().Be(ObjectCache.NoSlidingExpiration);
//        }

//        [Fact]
//        public void WithSlidingExpirationTest()
//        {
//            TimeSpan slidingExpiration = TimeSpan.FromMinutes(5);
            
//            var cachePolicy = CachePolicy.WithSlidingExpiration(slidingExpiration);
//            cachePolicy.Should().NotBeNull();
//            cachePolicy.Mode.Should().Be(CacheExpirationType.Sliding);
//            cachePolicy.AbsoluteExpiration.Should().Be(ObjectCache.InfiniteAbsoluteExpiration);
//            cachePolicy.SlidingExpiration.Should().Be(slidingExpiration);
//        }

//        [Fact]
//        public void WithDurationExpirationTest()
//        {
//            TimeSpan expirationSpan = TimeSpan.FromSeconds(30);
//            var cachePolicy = CachePolicy.WithDurationExpiration(expirationSpan);

//            cachePolicy.Should().NotBeNull();
//            cachePolicy.Mode.Should().Be(CacheExpirationType.Duration);
//            cachePolicy.AbsoluteExpiration.Should().Be(ObjectCache.InfiniteAbsoluteExpiration);
//            cachePolicy.SlidingExpiration.Should().Be(ObjectCache.NoSlidingExpiration);
//            cachePolicy.Duration.Should().Be(expirationSpan);
//        }

//    }
//}
