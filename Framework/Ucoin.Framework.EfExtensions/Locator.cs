using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucoin.Framework.Cache;
using Ucoin.Framework.EfExtensions.Batch;
using Ucoin.Framework.EfExtensions.Future;
using Ucoin.Framework.EfExtensions.Mapping;


namespace Ucoin.Framework.EfExtensions
{
    public class Locator
    {
        private static readonly Locator _instance = new Locator();
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Locator"/> class.
        /// </summary>
        public Locator()
        {
            _container = new Container();
            RegisterDefaults(_container);
        }

        /// <summary>
        /// Gets the current Locator <see cref="IContainer"/>.
        /// </summary>
        public static IContainer Current
        {
            get { return _instance.Container; }
        }

        /// <summary>
        /// Sets the <see cref="Current"/> <see cref="IContainer"/>.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> to set.</param>
        public static void SetContainer(IContainer container)
        {
            _instance.SetInnerContainer(container);
        }

        /// <summary>
        /// Gets the <see cref="IContainer"/> for this instance.
        /// </summary>
        protected IContainer Container
        {
            get { return _container; }
        }

        /// <summary>
        /// Sets the <see cref="IContainer"/> for this instance.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> for this instance.</param>
        protected void SetInnerContainer(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            _container = container;
        }

        /// <summary>
        /// Registers the default service provider resolvers.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> to register the default service resolvers with.</param>
        public static void RegisterDefaults(IContainer container)
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
