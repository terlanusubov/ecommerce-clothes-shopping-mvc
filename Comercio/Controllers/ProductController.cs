using Comercio.Data;
using Comercio.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult List([FromQuery]ProductListQueryModel request)
        {
            if(request == null) request = new ProductListQueryModel();
            return View(request);
        }
    }
}
