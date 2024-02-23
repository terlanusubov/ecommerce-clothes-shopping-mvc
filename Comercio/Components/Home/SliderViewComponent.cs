using Comercio.Data;
using Comercio.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Components.Home
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public SliderViewComponent(ApplicationDbContext context,
                             IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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

            return View(sliders);
        }
    }
}
