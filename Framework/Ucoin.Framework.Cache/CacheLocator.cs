using Ucoin.Framework.Dependency;
using Ucoin.Framework.Serialization;

namespace Ucoin.Framework.Cache
{
    public class CacheLocator : SimpleLocator
    {  
        public override void RegisterDefaults(IContainer container)
        {
            container.Register<CacheManager<AspNetCache>>(
                () => new CacheManager<AspNetCache>(t => { return new AspNetCache(); }));
            container.Register<CacheManager<StaticCache>>(
                () => new CacheManager<StaticCache>(t => { return new StaticCache(); }));
            container.Register<CacheManager<RedisCache>>(
                () => new CacheManager<RedisCache>(t => { return new RedisCache(SerializationHelper.Jil);}));
        }
    }
}
