using System;
using System.Collections.Concurrent;

namespace Ucoin.Framework.Dependency
{
    /// <summary>
    /// The default <see cref="IContainer"/> for resolving dependencies.
    /// </summary>
    public class SimpleContainer : IContainer
    {
        private readonly ConcurrentDictionary<Type, object> _factories;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        public SimpleContainer()
        {
            _factories = new ConcurrentDictionary<Type, object>();
        }

        /// <summary>
        /// Register the specified <paramref name="factory"/> for resolving <typeparamref name="TService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="factory">The factory <see langword="delegate"/> for resolving.</param>
        public virtual void Register<TService>(Func<TService> factory)
        {
            Type key = typeof(TService);
            _factories[key] = factory;
        }

        /// <summary>
        /// Resolves an instance for the specified <typeparamref name="TService"/> type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        /// A resolved instance of <typeparamref name="TService"/>.
        /// </returns>
        public virtual TService Resolve<TService>()
        {
            object factory;

            if (_factories.TryGetValue(typeof(TService), out factory))
            {
                return ((Func<TService>)factory)();
            }

            Type serviceType = typeof(TService);
            if (serviceType.IsInterface || serviceType.IsAbstract)
            {
                return default(TService);
            }

            try
            {
                return (TService)Activator.CreateInstance(serviceType);
            }
            catch
            {
                return default(TService);
            }
        }
    }
}