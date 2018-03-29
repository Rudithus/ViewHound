using System;
using System.Web;
using System.Web.Mvc;

namespace ViewHound.Mvc5
{
    public static class MvcApplicationExtensions
    {
        public static HoundConfiguration AddHound(this HttpApplication app)
        {
            var hound = new Hound();
            app.Application[name: $"Hound{Guid.NewGuid()}"] = hound;
            hound.StartTracking(app, ViewEngines.Engines);
            return new HoundConfiguration(app, hound);
        }
    }
    public class HoundConfiguration
    {
        public HttpApplication HttpApplication { get; }
        public IHound Hound { get; }
        public HoundConfiguration(HttpApplication httpApplication, IHound hound)
        {
            Hound = hound;
            HttpApplication = httpApplication;
        }
    }
}