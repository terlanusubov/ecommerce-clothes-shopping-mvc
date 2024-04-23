using Comercio.Areas.Admin.ViewModels;
using Comercio.Data;
using Comercio.DTOs;
using Comercio.Helper;
using Comercio.Interfaces;
using Comercio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookies")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IMemoryCache _memoryCache;

        private readonly IProductManager _productManager;

        public ProductController(ApplicationDbContext context,
                                IMemoryCache memoryCache,
                                IProductManager productManager)
        {
            _context = context;
            _productManager = productManager;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var vm = new ProductAddVm
            {
                ProductGet = await CreateProductGet()
            };

            return View(vm);
        }

        [HttpPost]
        public async  Task<JsonResult> Add([FromForm] ProductAddVm request)
        {
            var validationResult = await _productManager.ValidateProduct(request.ProductPost);

            if (!validationResult.Item1)
            {
                await FillModelState(validationResult.Item2);
                
                request.ProductGet = await CreateProductGet();

                return Json(new
                {
                    status = 400
                });
            }

            var productCreateResult = await _productManager.CreateProduct(request.ProductPost);

            if (!productCreateResult)
            {
                request.ProductGet = await CreateProductGet();

                return Json(new
                {
                    status = 400
                });
            }

            return Json(new { status = 200 });
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return View();
        }

        private async Task<ProductGetModel> CreateProductGet()
        {
            List<CategoryDto> res;

            if (!_memoryCache.TryGetValue("AdminProductAddCategories", out res))
            {
                res = await GeneralHelper.GetAllCategories(_context);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                              .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set("AdminProductAddCategories", res, cacheEntryOptions);

            }

            var genderTypes = await _context.GenderTypes.Select(c => new GenderTypeDto
            {
                GenderName = c.Name,
                GenderTypeId = c.Id
            }).ToListAsync();

            var getModel = new ProductGetModel();

            getModel.Categories = res;

            getModel.Genders = genderTypes;

            return getModel;
        }

        private async Task FillModelState(Dictionary<string,string> errors)
        {
            ModelState.Clear();

            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

        }

    }
}
