using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Helper
{
    public static class RenderPartialView
    {
        public static async Task<string> InvokeAsync(string viewName, 
                                                     ControllerContext _controllerContext,
                                                     ICompositeViewEngine _viewEngine,
                                                     ViewDataDictionary viewData,
                                                     ITempDataDictionary tempData,
                                                     object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = _controllerContext.ActionDescriptor.ActionName;
            viewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(_controllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    _controllerContext,
                    viewResult.View,
                    viewData,
                    tempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
