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
    /// <summary>
    /// Tracks view usage across the mvc application
    /// </summary>
    public class Hound : IHound
    {

        private ConcurrentDictionary<string, ulong> _viewUsage = new ConcurrentDictionary<string, ulong>();
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
            _viewUsage = new ConcurrentDictionary<string, ulong>(_directoryHelper
                .GetFiles(HostingEnvironment.ApplicationPhysicalPath, "*.cshtml", SearchOption.AllDirectories)
                .ToDictionary(f => f, _ => (ulong)0));
            var engines = viewEngines.Select(v => new ViewTrackerRazorEngine(v, this)).ToList();
            viewEngines.Clear();
            engines.ForEach(viewEngines.Add);
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

        /// <summary>
        /// Resets use statistics
        /// </summary>
        public void ResetUsage()
        {
            _viewUsage = new ConcurrentDictionary<string, ulong>(_viewUsage.ToDictionary(v => v.Key, _ => (ulong)0));
        }
    }
}
