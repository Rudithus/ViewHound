using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Jumanji.Framework.ViewTracker
{
    public class ViewTrackerRazorEngine : IViewEngine
    {
        private readonly IViewEngine _engine;
        private readonly IHound _hound;

        public ViewTrackerRazorEngine(IViewEngine engine, IHound hound)
        {
            _engine = engine;
            _hound = hound;
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var engineResult = _engine.FindPartialView(controllerContext, partialViewName, useCache);
            InterceptEngineResult(engineResult, controllerContext.HttpContext);
            return engineResult;
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var engineResult = _engine.FindView(controllerContext, viewName, masterName, useCache);
            InterceptEngineResult(engineResult, controllerContext.HttpContext);
            return engineResult;
        }
        private void InterceptEngineResult(ViewEngineResult result, HttpContextBase httpContext)
        {
            if (result.View is RazorView resultView)
            {
                _hound.AddViewUse(resultView.ViewPath);
            }
        }
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            _engine.ReleaseView(controllerContext, view);
        }
    }
}