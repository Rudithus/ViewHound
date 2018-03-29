using System.Web.Routing;
using System.Web.Http;

namespace ViewHound.Mvc5.UI
{
    public static class HoundConfigurationExtensions
    {
        public static void AddHoundUi(this HoundConfiguration houndConfiguration, RouteCollection routes)
        {
            var application = houndConfiguration.HttpApplication;
            
            routes.MapHttpRoute(
                 name: "houndroute",
                 routeTemplate: "hound",
                 defaults: null,
                 constraints: null,
                 handler: new HoundUiHttpHandler(houndConfiguration.Hound)
             );
        }
    }
}