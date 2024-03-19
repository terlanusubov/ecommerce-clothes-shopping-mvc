using Comercio.Data;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult List(int? categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return View();
        }
    }
}
