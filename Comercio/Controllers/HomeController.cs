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

		public IActionResult Index()
		{
			var sliders = _context.Sliders
								   .Select(s => new SliderDto
								   {
									   SliderId = s.Id,
									   Title = s.Title,
									   BackgroundImageUrl = _configuration["Folders:Sliders"] + s.BackgrounImageURL,
									   Text = s.Slogan,
									   Link = s.Link,
								   })
								   .OrderByDescending(s => s.SliderId)
								   .ToList();


			var categories = _context.Categories.Where(c => c.IsMainPage)
												.Select(c => new CategoryDto
												{
													Slogan = c.Slogan,
													
													CategoryId = c.Id,
													
													BackgroundImageUrl = _configuration["Folders:Categories"] + c.BackgroundImageURL,
													
													Name = c.Name,
													
													Priority = c.Priority ?? 0
												})
												.OrderBy(c=>c.Priority)
												.ToList();



			var vm = new HomeIndexVm();

			vm.Sliders = sliders;

			vm.Categories = categories;

			return View(vm);
		}
	}
}
