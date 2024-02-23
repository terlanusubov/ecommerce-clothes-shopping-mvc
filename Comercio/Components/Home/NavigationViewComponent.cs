using Comercio.Data;
using Comercio.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Components.Home
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public NavigationViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
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

            return View(navCategories);
        }
    }
}
