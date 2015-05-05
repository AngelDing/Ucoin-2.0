using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Collections.Generic;
using Ucoin.Conference.Admin.Resolver;
using Ucoin.Framework.Web;

namespace Ucoin.Conference.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MaintenanceMode.RefreshIsInMaintainanceMode();
            DatabaseSetup.Initialize();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(new UnityResolver(Container.InitUnityContainer()));
            ModelBinders.Binders.DefaultBinder = new DefaultModelBinder();
        }
    }
}
