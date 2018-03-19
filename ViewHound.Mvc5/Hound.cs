using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace ViewHound.Mvc5
{
    public class Hound : IHound
    {

        private ConcurrentDictionary<string, int> _viewUsage;
        private readonly IDirectoryHelper _directoryHelper;
        public Hound() : this(new DirectoryHelper()) { }
        public Hound(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        /// <summary>
        /// Wraps existing view engines with ViewTrackerRazorEngine.
        /// </summary>
        /// <param name="application">Mvc Application</param>
        /// <param name="viewEngines">View Engines</param>
        public void StartTracking(HttpApplication application, ViewEngineCollection viewEngines)
        {
            _viewUsage = new ConcurrentDictionary<string, int>(_directoryHelper
                .GetFiles(HostingEnvironment.ApplicationPhysicalPath, "*.cshtml", SearchOption.AllDirectories)
                .ToDictionary(f => f, _ => 0));
            var engines = viewEngines.Select(v => new ViewTrackerRazorEngine(v, this)).ToList();
            viewEngines.Clear();
            engines.ForEach(viewEngines.Add);
        }

        public void UseHoundPage(RouteCollection routes, ControllerBuilder controllerBuilder)
        {
            var handler = new MvcRouteHandler();            
            routes.MapRoute(
                name: "HoundRoute",
                url: "Hound/{action}",
                namespaces: new[] { typeof(HoundController).Namespace });

            var currentControllerFactory = controllerBuilder.GetControllerFactory();
            var houndControllerFactory = new HoundControllerFactory(currentControllerFactory, this);
            controllerBuilder.SetControllerFactory(houndControllerFactory);
        }

        /// <summary>
        /// Records View Use
        /// </summary>
        /// <param name="viewPath">path of the used view</param>
        public void RecordViewUse(string viewPath)
        {
            if (_viewUsage.ContainsKey(viewPath))
            {
                _viewUsage[viewPath]++;
            }
            else
            {
                _viewUsage[viewPath] = 1;
            }
        }

        /// <summary>
        /// Returns used view paths and their use counts
        /// </summary>
        /// <returns>Used view paths and counts</returns>
        public List<ViewUse> GetUsedViewsLog()
        {
            return _viewUsage
                .Where(v => v.Value > 0)
                .Select(v => new ViewUse { ViewPath = v.Key, UseCount = v.Value })
                .ToList();
        }
        /// <summary>
        /// Returns unused view paths and their use counts
        /// </summary>
        /// <returns>Unused view paths and counts</returns>
        public List<ViewUse> GetUnUsedViewsLog()
        {
            return _viewUsage
                .Where(v => v.Value == 0)
                .Select(v => new ViewUse() { UseCount = v.Value, ViewPath = v.Key })
                .ToList();
        }
        /// <summary>
        /// Returns all view paths and their use counts
        /// </summary>
        /// <returns>All view paths and counts</returns>
        public List<ViewUse> GetAllViewsLog()
        {
            return _viewUsage
                .Select(v => new ViewUse() { UseCount = v.Value, ViewPath = v.Key })
                .ToList();
        }
    }

    public class HoundOptions
    {
        public bool LogAfterEveryRequest { get; internal set; }
        public string BasePath { get; internal set; } = HostingEnvironment.ApplicationPhysicalPath;
        public HoundOptions()
        {

        }

        public HoundOptions(bool logAfterEveryRequest, string basePath)
        {
            BasePath = basePath;
            LogAfterEveryRequest = logAfterEveryRequest;
        }
    }

    public class HoundControllerFactory : IControllerFactory
    {
        private readonly IHound _hound;
        private readonly IControllerFactory _controllerFactory;

        public HoundControllerFactory(IControllerFactory controllerFactory, IHound hound)
        {
            _controllerFactory = controllerFactory;
            _hound = hound;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            return controllerName == nameof(HoundController) ? new HoundController(_hound) : _controllerFactory.CreateController(requestContext, controllerName);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {

            return _controllerFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            _controllerFactory.ReleaseController(controller);
        }
    }
}
