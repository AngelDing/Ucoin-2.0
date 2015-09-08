using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ucoin.Conference.Web.Resolver;
using Ucoin.Framework.Web;
using Ucoin.Framework.Web.Resolver;

namespace Ucoin.Conference.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            MaintenanceMode.RefreshIsInMaintainanceMode();
            DatabaseSetup.Initialize();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(new UnityMvcResolver(Container.InitUnityContainer()));
            ModelBinders.Binders.DefaultBinder = new DefaultModelBinder();
        }
    }
}
