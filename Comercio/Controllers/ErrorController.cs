using Microsoft.AspNetCore.Mvc;

namespace Comercio.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Internal()
        {
            return View();
        }
    }
}
