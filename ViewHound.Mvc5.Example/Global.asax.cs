using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ViewHound.Mvc5.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Hound Hound = new Hound();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Hound.StartTracking(this, ViewEngines.Engines);
            Hound.UseHoundPage(RouteTable.Routes, ControllerBuilder.Current);
        }
    }
}
