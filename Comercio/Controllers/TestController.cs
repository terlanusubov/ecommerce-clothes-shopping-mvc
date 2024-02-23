using Comercio.Data;
using Comercio.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var navCategories = _context.Categories
                                            .Where(c => c.ParentId == null)
                                            .Select(c => new CategoryDto
                                            {
                                                CategoryId = c.Id,
                                                Name = c.Name,
                                                Priority = c.Priority ?? 0
                                            })
                                            .ToList();

            ViewBag.Categories = navCategories;

            return View();
        }
    }
}
