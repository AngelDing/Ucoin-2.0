using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using Ucoin.Framework.Web.Activator;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(Ucoin.Authority.Site.UnityWebActivator), "Start")]
[assembly: ApplicationShutdownMethod(typeof(Ucoin.Authority.Site.UnityWebActivator), "Shutdown")]

namespace Ucoin.Authority.Site
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityWebActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            var removeItem = FilterProviders.Providers
                .OfType<FilterAttributeFilterProvider>().First();
            FilterProviders.Providers.Remove(removeItem);
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));            
            DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
           
            container.Dispose();
        }
    }
}