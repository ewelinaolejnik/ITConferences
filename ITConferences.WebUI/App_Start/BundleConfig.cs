using System.Web.Optimization;

namespace ITConferences.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/conference").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/jquery.multi-select.js",
                "~/Scripts/jquery.multiselect.filter.js",
                "~/Scripts/conference.js"));

            bundles.Add(new ScriptBundle("~/bundles/rating").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/star-rating.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/home-page.js"));

            bundles.Add(new ScriptBundle("~/bundles/manage").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/jquery.multi-select.js",
                "~/Scripts/jquery.multiselect.filter.js",
                "~/Scripts/manage.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/create-conference").Include(
                "~/Content/metro-bootstrap.css",
                "~/Content/jquery-ui.min.css",
                "~/Content/multi-select.css",
                "~/Content/jquery.multiselect.filter.css",
                "~/Content/Custom/conference.css"));

            bundles.Add(new StyleBundle("~/Content/details").Include(
                "~/Content/metro-bootstrap.css",
                "~/Content/star-rating.min.css",
                "~/Content/Custom/conference.css",
                "~/Content/Custom/user.css"));

            bundles.Add(new StyleBundle("~/Content/alerts").Include(
                "~/Content/metro-bootstrap.css",
                "~/Content/Custom/alerts.css"));
        }
    }
}