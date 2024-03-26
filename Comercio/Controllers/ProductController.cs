using Comercio.Components.Product;
using Comercio.Data;
using Comercio.ServiceModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Text.Encodings.Web;

namespace Comercio.Controllers
{
    public class ProductController : Controller
    {
        private readonly IViewComponentHelper _viewComponentHelper;
        public ProductController(IViewComponentHelper viewComponentHelper)
        {
            _viewComponentHelper = viewComponentHelper;
        }

        public IActionResult List([FromQuery]ProductListQueryModel request)
        {
            if(request == null) request = new ProductListQueryModel();
            return View(request);
        }

        [HttpGet]
        public async Task<JsonResult> Filter([FromQuery] ProductListQueryModel request)
        {
			var viewComponentResult = await _viewComponentHelper.InvokeAsync(typeof(ProductListViewComponent), request);

			var viewContent = RenderViewToString(viewComponentResult);

			return Json(new
			{
				data = viewContent,
				status = 200,
				currentPage = request.Page
			});
		}

        private string RenderViewToString(IHtmlContent viewContent)
        {
            using (var writer = new StringWriter())
            {
                viewContent.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
