using Ucoin.Framework.Cache;
using Ucoin.Framework.Dependency;
using Ucoin.Framework.EfExtensions.Batch;
using Ucoin.Framework.EfExtensions.Future;
using Ucoin.Framework.EfExtensions.Mapping;

namespace Ucoin.Framework.EfExtensions
{
    public class EfLocator : SimpleLocator
    {        
        /// <summary>
        /// Registers the default service provider resolvers.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> to register the default service resolvers with.</param>
        public override void RegisterDefaults(IContainer container)
        {
            container.Register<IMappingProvider>(() => new MetadataMappingProvider());
            container.Register<IBatchInsert>(() => new BatchInsertProvider());
            container.Register<IBatchUpdate>(() => new BatchUpdateProvider());
            container.Register<IBatchDelete>(() => new BatchDeleteProvider());
            container.Register<ICacheManager>(() => new CacheManager<StaticCache>(t => { return new StaticCache(); }));
            container.Register<IFutureRunner>(() => new FutureRunner());
        }
    }
}
