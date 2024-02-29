using Comercio.Data;
using Comercio.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Components.Home
{
    public class TrendyProductsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TrendyProductsViewComponent(ApplicationDbContext context,
                                         IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var trendyProducts = await _context
                                        .TrendyProducts
                                        .Select(c=> new TrendyProductDto
                                        {
                                            ProductId = c.ProductId,
                                            Name = c.Name,
                                            Slug = c.Slug,
                                            ImageURL = _configuration["Folders:TrendyProducts"] + c.ImageURL,
                                            Price = c.Price,
                                            Description = c.Description
                                        })    
                                        .ToListAsync();

            return View(trendyProducts);
        }
    }
}
