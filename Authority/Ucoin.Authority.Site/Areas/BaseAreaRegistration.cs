using System.Web.Http;
using System.Web.Mvc;
using Ucoin.Framework.Web.Api;

namespace Ucoin.Authority.Site.Areas
{
    public abstract class BaseAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "Ucoin.Authority.Site.Areas." + this.AreaName + ".Controllers" }
            );

            var namespaceName = new string[] 
            { 
                string.Format("Ucoin.Authority.Site.Areas.{0}.Controllers", this.AreaName) 
            };

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                this.AreaName + "Api",
                "api/" + this.AreaName + "/{controller}/{action}/{id}",
                new
                {
                    area = this.AreaName,
                    action = RouteParameter.Optional,
                    id = RouteParameter.Optional,
                    namespaceName = namespaceName
                },
                new
                { 
                    action = new StartWithConstraint()
                }
            );
        }
    }
}