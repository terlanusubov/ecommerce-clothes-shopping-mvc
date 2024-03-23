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

        public ProductListViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProductListQueryModel request)
        {
            string userId = null;
           
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                userId = HttpContext.User.Claims.Where(c => c.Type == "Id").FirstOrDefault().Value;
            }

            var query = _context.Products.Where(c => (request.CategoryId == null || c.CategoryId == request.CategoryId));

            var count = await query.CountAsync();

            query = query.OrderByDescending(c => c.Created);

            //TODO: change hard code
            query = query.Skip((request.Page - 1) * 10).Take(10);

            query = query.Include(c => c.ProductPhotos)
                         .Include(c=>c.UserWishlists);

            //TODO: change hard code of image url
            var products = await query.Select(c => new ProductDto
            {
                ProductId = c.Id,
                
                Slug = c.Slug,
                
                Description = c.Description,
                
                ImageURL = "https://localhost:7024/uploads/products/" + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageURL).FirstOrDefault(),
                
                Name = c.Name,
                
                Price = c.SellAmount,
               
                IsWishlist = userId == null? false : c.UserWishlists.Any(a=>a.UserId.ToString() == userId) 
                
                //TODO: if product has discount then subtract it and return discount too
           
            }).ToListAsync();

            //TODO: check if product doesnt have main image or any image

            var totalPage = (int)Math.Ceiling(count / (decimal)10); //TODO: change take hard code number

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
