using System.Web;
using System.Web.Mvc;

namespace ViewHound.Mvc5
{
    public class ViewTrackerRazorEngine : IViewEngine
    {
        private readonly IViewEngine _engine;
        private readonly IHound _hound;

        /// <summary>
        /// IViewEngine wrapper that records view usages
        /// </summary>
        /// <param name="engine">View Engine to be wrapped</param>
        /// <param name="hound">Parent hound to keep track of usage</param>
        public ViewTrackerRazorEngine(IViewEngine engine, IHound hound)
        {
            _engine = engine;
            _hound = hound;
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var engineResult = _engine.FindPartialView(controllerContext, partialViewName, useCache);
            InterceptEngineResult(engineResult);
            return engineResult;
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var engineResult = _engine.FindView(controllerContext, viewName, masterName, useCache);
            InterceptEngineResult(engineResult);
            return engineResult;
        }

        /// <summary>
        /// Checks if the ViewEngineResult is succesful and records it if it is
        /// </summary>
        /// <param name="result"></param>
        private void InterceptEngineResult(ViewEngineResult result)
        {
            if (result.View is RazorView resultView)
            {
                _hound.RecordViewUse(resultView.ViewPath);
            }
        }
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            _engine.ReleaseView(controllerContext, view);
        }
    }
}