using Comercio.Data;
using Comercio.DTOs;
using Comercio.ServiceModels;
using Comercio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Components.Product
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public ProductListViewComponent(ApplicationDbContext context,
                                        IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProductListQueryModel request)
        {
            //TODO: must work for parent category id filter

            string userId = null;
           
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userId = HttpContext.User.Claims.Where(c => c.Type == "Id").FirstOrDefault().Value;
            }

            var takeNumber = request.Take ?? Convert.ToInt32(_configuration["List:Products"]);

            var productImageBasePath = _configuration["Folders:Products"];

            var query = _context.Products.Where(c => (request.CategoryId == null || c.CategoryId == request.CategoryId));

            var count = await query.CountAsync();

            query = query.OrderByDescending(c => c.Created);

            query = query.Skip((request.Page - 1) * takeNumber).Take(takeNumber);

            query = query.Include(c => c.ProductPhotos)
                         .Include(c=>c.UserWishlists);

            var products = await query.Select(c => new ProductDto
            {
                ProductId = c.Id,
                
                Slug = c.Slug,
                
                Description = c.Description,
                
                ImageURL = productImageBasePath + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageURL).FirstOrDefault(),
                
                Name = c.Name,
                
                Price = c.SellAmount,
               
                IsWishlist = userId == null? false : c.UserWishlists.Any(a=>a.UserId.ToString() == userId) ,

                Discount = c.Discount,
                
                DiscountedPrice = c.Discount != null ? c.SellAmount - (c.SellAmount * (double)c.Discount / 100) : null
            }).ToListAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var vm = new ProductListVm
            {
                CurrentPage = request.Page,
                TotalPage = totalPage,
                Products = products
            };
            
            return View(vm);

        }
    }
}
