using System.Web;
using System.Web.Mvc;

namespace Ucoin.RightsManagement.AdminSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
