using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CovidPayTracking.Helper
{
    public static class ConvertViewEmailTemplate
    {
        public static string RenderViewToString<T>(ControllerContext controllerContext, string viewName, T model)
        {
            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);
            if(viewEngineResult.View == null)
            {
                throw new Exception("View file not found. Searched locations: \r\n" + viewEngineResult.SearchedLocations);
            }
            else
            {
                IView view = viewEngineResult.View;
                using (var stringWriter = new StringWriter())
                {
                    var viewContext = new ViewContext (controllerContext, view, new ViewDataDictionary<T>(model), new TempDataDictionary(), stringWriter);
                    view.Render(viewContext, stringWriter);
                    return stringWriter.ToString();
                }
            }
        }
    }
}