using Comercio.Data;
using Comercio.DTOs;
using Comercio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Controllers
{
	public class HomeController : Controller
	{
        public IActionResult Index() => View();
	}
}
