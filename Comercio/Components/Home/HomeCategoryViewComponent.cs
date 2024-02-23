using Comercio.Data;
using Comercio.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Components.Home
{
    public class HomeCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public HomeCategoryViewComponent(ApplicationDbContext context,
                             IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _context.Categories.Where(c => c.IsMainPage)
                                                 .Select(c => new CategoryDto
                                                 {
                                                     Slogan = c.Slogan,

                                                     CategoryId = c.Id,

                                                     BackgroundImageUrl = _configuration["Folders:Categories"] + c.BackgroundImageURL,

                                                     Name = c.Name,

                                                     Priority = c.Priority ?? 0
                                                 })
                                                 .OrderBy(c => c.Priority)
                                                 .ToList();

            return View(categories);
        }
    }
}
