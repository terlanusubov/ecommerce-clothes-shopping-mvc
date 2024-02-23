using Comercio.Data;
using Comercio.DTOs;
using Comercio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		private readonly IConfiguration _configuration;

		public HomeController(ApplicationDbContext context,
							 IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		public IActionResult Index() => View();
		
	}
}
