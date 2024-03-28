using Azure.Core;
using Comercio.Data;
using Comercio.DTOs;
using Comercio.Interfaces;
using Comercio.ServiceModels;
using Comercio.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Services
{
    public class ProductManager : IProductManager
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly HttpContext _httpContext;
        public ProductManager(IHttpContextAccessor httpContextAccessor,
                              IConfiguration configuration,
                              ApplicationDbContext context)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _configuration = configuration;
            _context = context;
        }
        public async Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request)
        {
            //TODO: must work for parent category id filter

            string userId = null;

            if (_httpContext.User.Identity.IsAuthenticated)
            {
                userId = _httpContext.User.Claims.Where(c => c.Type == "Id").FirstOrDefault().Value;
            }

            var takeNumber = request.Take ?? Convert.ToInt32(_configuration["List:Products"]);

            var productImageBasePath = _configuration["Folders:Products"];
            var query = _context.Products.Where(c => (request.CategoryId == null || c.CategoryId == request.CategoryId));
            var count = await query.CountAsync();

            query = query.OrderByDescending(c => c.Created);

            query = query.Skip((request.Page - 1) * takeNumber).Take(takeNumber);

            var productCount = await query.CountAsync();

            query = query.Include(c => c.ProductPhotos)
                         .Include(c => c.UserWishlists);

            var products = await query.Select(c => new ProductDto
            {
                ProductId = c.Id,

                Slug = c.Slug,

                Description = c.Description,

                ImageURL = productImageBasePath + c.ProductPhotos.Where(a => a.IsMain).Select(a => a.ImageURL).FirstOrDefault(),

                Name = c.Name,

                Price = c.SellAmount,

                IsWishlist = userId == null ? false : c.UserWishlists.Any(a => a.UserId.ToString() == userId),

                Discount = c.Discount,

                DiscountedPrice = c.Discount != null ? c.SellAmount - (c.SellAmount * (double)c.Discount / 100) : null
            }).ToListAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)takeNumber);

            var vm = new ProductListVm
            {
                CurrentPage = request.Page,
                TotalPage = totalPage,
                Products = products,
                ProductCount = productCount
            };

            return vm;
        }
    }
}
