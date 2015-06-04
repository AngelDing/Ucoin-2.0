using Moq;
using System;
using StackExchange.Redis;
using System.Web;
using Xunit;
using FluentAssertions;
using Ucoin.Framework;
using Ucoin.Framework.RedisSession;

namespace Ucoin.Framework.Test.RedisSession
{
    public class ExpirationTests : DisposableObject
    {
        private static string REDIS_SERVER = "127.0.0.1:6380";
        private static int REDIS_DB = 13;
        private static TimeSpan TIMEOUT = new TimeSpan(1, 0, 0);
        private static string SESSION_ID = "SESSION_ID";
        static ConfigurationOptions _redisConfigOpts;
        private IDatabase db;

        public ExpirationTests()
        {
            _redisConfigOpts = ConfigurationOptions.Parse(REDIS_SERVER);
            RedisConnectionConfig.GetSERedisServerConfigDbIndex = @base =>
                new Tuple<string, int, ConfigurationOptions>("SessionConnection", REDIS_DB, _redisConfigOpts);
            RedisSessionConfig.SessionTimeout = TIMEOUT;

            // StackExchange Redis client
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(REDIS_SERVER);
            db = redis.GetDatabase(REDIS_DB);
        }

        [Fact]
        public void ExpirationSet_AsExpected()
        {
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.Setup(x => x.Cookies).Returns(new HttpCookieCollection()
            {
                new HttpCookie(RedisSessionConfig.SessionHttpCookieName, SESSION_ID)
            });
            mockHttpContext.Setup(x => x.Request).Returns(mockHttpRequest.Object);


            using (var sessAcc = new RedisSessionAccessor(mockHttpContext.Object))
            {
                sessAcc.Session["MyKey"] = DateTime.UtcNow;
            }

            // Assert directly using Stackexchange.Redis
            var ttl = db.KeyTimeToLive(SESSION_ID);
            
            // We should not have a null here
            ttl.Should().HaveValue();
            ttl.Value.Should().BeLessOrEqualTo(TIMEOUT);
            ttl.Value.Minutes.Should().BeGreaterThan(0);
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                db.KeyDelete(SESSION_ID);
            }
        }
    }
}
