using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ViewHound.Mvc5
{
    public interface IHound
    {
        void StartTracking(HttpApplication application, ViewEngineCollection viewEngines);
        List<ViewUse> GetUsedViewsLog();
        List<ViewUse> GetUnUsedViewsLog();
        List<ViewUse> GetAllViewsLog();
        void RecordViewUse(string viewPath);
    }
}