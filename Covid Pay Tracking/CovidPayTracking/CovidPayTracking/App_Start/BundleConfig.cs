using System.Web;
using System.Web.Optimization;

namespace CovidPayTracking
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                    "~/Scripts/moment.min.js",
                     "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqdatatables").Include(
                   "~/Scripts/DataTables/jquery.dataTables.min.js",
                   "~/Scripts/DataTables/dataTables.bootstrap.js",
                   "~/Scripts/DataTables/dataTables.fixedHeader.min.js",
                   "~/Scripts/DataTables/dataTables.fixedColumns.min.js",
                    "~/Scripts/DataTables/dataTables.buttons.min.js",
                   "~/Scripts/DataTables/dataTables.select.min.js",
                   "~/Scripts/jquery.dataTables.yadcf.js",
                    "~/Scripts/notify.min.js",
                    "~/Scripts/json2.js",
                    "~/Scripts/jQuery.dataTables.sortDateExtend.js"));

            bundles.Add(new StyleBundle("~/Content/jqdatatables").Include(
                      "~/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/Content/DataTables/css/fixedColumns.bootstrap.min.css",
                      "~/Content/jquery.dataTables.yadcf.css"));
        }
    }
}
