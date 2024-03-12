using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Roles = "Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var result = Test();

            return View();
        }

        public (string,int) Test()
        {
            return ("salam", 4);
        }
    }
}
