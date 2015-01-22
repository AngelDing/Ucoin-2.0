using System.Web.Optimization;

namespace Ucoin.Authority.Site
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery/jquery-2.1.3.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                "~/Content/easyui/easyloader-1.4.1.js?them=default",
                "~/Content/easyui/locale/easyui-lang-zh_CN.js"));

            bundles.Add(new ScriptBundle("~/bundles/library").Include(
                "~/Scripts/knockout/knockout-3.2.0.min.js",
                "~/Scripts/knockout/knockout.bindings.js",
                "~/Scripts/core/jquery.easyui.fix.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/base.css",
                "~/Content/css/960/fluid/reset.css",
                "~/Content/css/960/fluid/text.css",
                "~/Content/css/960/fluid/grid.css",
                "~/Content/css/btns/css3btn.css",
                "~/Content/css/btns/sexybuttons.css",
                "~/Content/css/hack/fix.css",
                "~/Content/css/icon/icon.css"));
        }
    }
}
