using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewHound
{
    internal class TrackerViewEngine : IViewEngine
    {
        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            if (context.HttpContext.Items["Views"] is Dictionary<string, int> views)
            {
                if (!views.ContainsKey(viewName)) views.Add(viewName, 0);
                views[viewName]++;
            }
            return ViewEngineResult.NotFound(viewName, new List<string> { "too long, did not search" });
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            return ViewEngineResult.NotFound(viewPath, new List<string> { "too long, did not search" });
        }
    }
}