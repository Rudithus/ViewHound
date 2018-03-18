using System.Web;
using System.Web.Mvc;

namespace ViewHound.Mvc5.Example
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
