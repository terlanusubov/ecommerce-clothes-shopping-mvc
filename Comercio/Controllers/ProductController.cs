using Comercio.Components.Product;
using Comercio.Data;
using Comercio.Helper;
using Comercio.Interfaces;
using Comercio.ServiceModels;
using Comercio.ViewModels;
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
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IProductManager _productManager;
        public ProductController(IProductManager productManager,
                                ICompositeViewEngine viewEngine)
        {
            _productManager = productManager;
            _viewEngine = viewEngine;
        }

        public IActionResult List([FromQuery]ProductListQueryModel request)
        {
            if(request == null) request = new ProductListQueryModel();
            return View(request);
        }

        [HttpGet]
        public async Task<JsonResult> Filter([FromQuery] ProductListQueryModel request)
        {
            try
            {
                var vm = await _productManager.GetFilteredProducts(request);

                var html = await RenderPartialView.InvokeAsync("_ProductListPartial",
                                                                ControllerContext,
                                                                _viewEngine,
                                                                ViewData,
                                                                TempData,
                                                                vm);
                return Json(new
                {
                    data = html,
                    status = 200,
                    productCount = vm.ProductCount,
                    totalPage = vm.TotalPage,
                    currentPage = request.Page
                });
            }
            catch (Exception exp)
            {
                return Json(new
                {
                    error = "xeta bas verdi",
                    status = 500
                });
            }
           
		}

        [HttpGet]
        public async Task<IActionResult> Detail(Guid productId)
        {
            var productGetResult = await _productManager.GetProductById(productId);

            if(productGetResult is null)
            {
                return Content("bele bir mehsul yoxdur");
            }

            var vm = new ProductDetailVm();

            vm.Product = productGetResult;

            return View(vm);
        }
    }
}
