using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using Ucoin.Framework.Utils;

namespace Ucoin.Framework.Web.Resolver
{
    public class BaseResolver : DisposableObject
    {
        protected IUnityContainer container;

        public BaseResolver(IUnityContainer container)
        {
            if (container == null)
            {
                GuardHelper.ArgumentNotNull(() => container);
            }
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                container.Dispose();
            }
        }
    }
}
