using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ViewHound.Mvc5.UI;

namespace ViewHound.Mvc5.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.AddHound()
                .AddHoundUi(RouteTable.Routes);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
    }
}
