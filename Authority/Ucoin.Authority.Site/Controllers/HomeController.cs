using System.Collections.Generic;
using System.Web.Mvc;

namespace Ucoin.Authority.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "XXX管理系统";
            ViewBag.UserName = "Jacky";
            ViewBag.Settings = GetCurrentUserSettings();

            return View();
        }

        private Dictionary<string, object> GetCurrentUserSettings()
        {
            var result = new Dictionary<string, object>();

            result.Add("theme", "default");
            result.Add("navigation", "accordion");
            return result;
        }
    }
}