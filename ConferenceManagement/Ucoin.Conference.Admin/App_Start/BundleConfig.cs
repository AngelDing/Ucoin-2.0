using System.Web;
using System.Web.Optimization;

namespace Ucoin.Conference.Admin
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //           "~/Scripts/thirdparty/jquery-{version}.js",
            //           "~/Scripts/thirdparty/jquery.form.js",
            //           "~/Scripts/thirdparty/jquery.validate.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/thirdparty/bootstrap.js"));

            //bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
            //       "~/Scripts/thirdparty/knockout-3.3.0.js",
            //       "~/Scripts/thirdparty/knockout.mapping-2.4.1.js",
            //       "~/Scripts/thirdparty/knockout.validation.js",
            //       "~/Scripts/perpetuum.knockout.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/bootstrap-theme.css",
            //          "~/Content/Site.css"));

            ////非开发环境强制开启捆绑和缩小
            //BundleTable.EnableOptimizations = true;
        }
    }
}
