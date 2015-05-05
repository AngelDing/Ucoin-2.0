

using System.Web.Mvc;
namespace Ucoin.Framework.Web
{
    public class MaintenanceModeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (MaintenanceMode.IsInMaintainanceMode)
            {
                filterContext.Result = new ViewResult { ViewName = "MaintenanceMode" };
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
