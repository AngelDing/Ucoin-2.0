
namespace Ucoin.Framework.Redis
{
    public static class RedisWrapperFactory
    {
        public static IRedisWrapper GetRedisWrapper()
        {
            return new StackExchangeRedisWrapper();
        }
    }
}
