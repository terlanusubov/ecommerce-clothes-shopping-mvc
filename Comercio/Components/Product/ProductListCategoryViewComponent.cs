using Comercio.Data;
using Comercio.DTOs;
using Comercio.Helper;
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
                res = await GeneralHelper.GetCategoryTree(_context);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                              .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set("Categories", res, cacheEntryOptions);

            }

            return View(res);
        }

       
    }
}
