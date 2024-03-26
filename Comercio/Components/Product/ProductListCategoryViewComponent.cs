using Comercio.Data;
using Comercio.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Comercio.Components.Product
{
    public class ProductListCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public ProductListCategoryViewComponent(ApplicationDbContext context,
                                                IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CategoryDto> res;

            if (!_memoryCache.TryGetValue("Categories", out res))
            {
                res = await GetCategoryTree();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                              .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set("Categories", res, cacheEntryOptions);

            }

            return View(res);
        }

        private async Task<List<CategoryDto>> GetCategoryTree(int? parentId = null)
        {

            List<CategoryDto> result = new List<CategoryDto>();

            var categories = await _context.Categories.Where(c => c.ParentId == parentId)
                                                        .Select(c => new CategoryDto
                                                        {
                                                            CategoryId = c.Id,
                                                            Slogan = c.Slogan,
                                                            BackgroundImageUrl = c.BackgroundImageURL,
                                                            Name = c.Name,
                                                            ParentId = c.ParentId,
                                                            Priority = c.Priority ?? 0
                                                        })
                                                        .ToListAsync();
            foreach (var category in categories)
            {
                var children = await _context.Categories.Where(c => c.ParentId == category.CategoryId).Select(c => new CategoryDto
                {
                    CategoryId = c.Id,
                    Slogan = c.Slogan,
                    BackgroundImageUrl = c.BackgroundImageURL,
                    Name = c.Name,
                    ParentId = c.ParentId,
                    Priority = c.Priority ?? 0
                }).ToListAsync();

                if (children.Count > 0)
                {
                    category.Children.AddRange(await GetCategoryTree(category.CategoryId));
                }

            }

            result.AddRange(categories);

            return result;

        }
    }
}
