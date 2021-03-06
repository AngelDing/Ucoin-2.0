﻿using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ucoin.Authority.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapPageRoute("Report", "report", "~/Content/page/report.aspx");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Ucoin.Authority.Site.Controllers" }
            );

            //for dynamic model binder
            ModelBinders.Binders.Add(typeof(JObject), new JObjectModelBinder());
        }
    }
}
