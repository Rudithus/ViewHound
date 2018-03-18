using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Jumanji.Framework.ViewTracker
{
    public class Hound : IHound
    {

        private readonly IDictionary<string, int> _viewUsage;
        private readonly HoundOptions _houndOptions;
        private bool _tracking;
        public Hound() : this(new DirectoryHelper(), new HoundOptions()) { }
        public Hound(IDirectoryHelper directoryHelper) : this(directoryHelper, new HoundOptions()) { }
        public Hound(IDirectoryHelper directoryHelper, HoundOptions houndOptios)
        {
            _houndOptions = houndOptios;
            _viewUsage = directoryHelper.GetFiles(_houndOptions.BasePath, "*.cshtml", SearchOption.AllDirectories).ToDictionary(f => f, _ => 0);
        }

        public void StartTracking(HttpApplication application, ViewEngineCollection viewEngines)
        {
            application.BeginRequest += Application_BeginRequest;

            application.EndRequest += Application_EndRequest;

            if (_tracking) return;
            var engines = viewEngines.Select(v => new ViewTrackerRazorEngine(v, this)).ToList();

            viewEngines.Clear();
            engines.ForEach(viewEngines.Add);



            _tracking = true;
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            var httpContextItems = ((HttpApplication)sender).Context.Items;
            httpContextItems.Add("ViewResults", new List<RazorView>());
        }

        private void Application_EndRequest(object source, EventArgs e)
        {
            var httpContextItems = ((HttpApplication)source).Context.Items;
            var viewResults = httpContextItems["ViewResults"] as List<RazorView>;


        }

        public void AddViewUse(string viewPath)
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

        public string GetUsedViewsLog()
        {
            return string.Join(Environment.NewLine,
                _viewUsage
                    .Where(v => v.Value > 0)
                    .Select(v => $"{v.Key} : {v.Value}"));
        }

        public string GetUnUsedViewsLog()
        {
            return string.Join(Environment.NewLine,
                _viewUsage
                    .Where(v => v.Value == 0)
                    .Select(v => $"{v.Key} : {v.Value}"));
        }

        public string GetAllViewsLog()
        {
            return string.Join(Environment.NewLine,
                _viewUsage
                    .Select(v => $"{v.Key} : {v.Value}"));
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

    public enum LoggingType
    {
        LogUsed,
        LogUnused,
        LogAll
    }
    public interface IHound
    {
        void StartTracking(HttpApplication application, ViewEngineCollection viewEngines);
        string GetUsedViewsLog();
        string GetUnUsedViewsLog();
        string GetAllViewsLog();
        void AddViewUse(string viewPath);
    }
}
