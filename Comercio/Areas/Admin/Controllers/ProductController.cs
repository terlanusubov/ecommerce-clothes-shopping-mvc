using Comercio.Areas.Admin.ViewModels;
using Comercio.Data;
using Comercio.DTOs;
using Comercio.Helper;
using Comercio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookies")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IMemoryCache _memoryCache;

        public ProductController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
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
            List<CategoryDto> res;

            if (!_memoryCache.TryGetValue("Categories", out res))
            {
                res = await GeneralHelper.GetCategoryTree(_context);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                              .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _memoryCache.Set("Categories", res, cacheEntryOptions);

            }

            var genderTypes = await _context.GenderTypes.Select(c => new GenderTypeDto
            {
                GenderName = c.Name,
                GenderTypeId = c.Id
            }).ToListAsync();

            var vm = new ProductAddVm
            {
                ProductGet = new ProductGetModel
                {
                    Categories = res,
                    Genders = genderTypes
                }
            };

            return View(vm);
        }

        [HttpPost]
        public async  Task<IActionResult> Add([FromForm] ProductAddVm request)
        {
            //TODO : add validation

            try
            {
               await _context.Database.BeginTransactionAsync();

                #region Create product entity
                var product = new Product();

                product.Name = request.ProductPost.Name;
                product.Barcode = request.ProductPost.Barcode;
                product.CategoryId = request.ProductPost.CategoryId;
                product.GenderTypeId = request.ProductPost.GenderTypeId;
                product.Description = request.ProductPost.Description;
                product.SellAmount = request.ProductPost.SellAmount;
                product.BuyAmount = request.ProductPost.BuyAmount;
                product.BuyLimit = request.ProductPost.BuyLimit ?? 10; //TODO change hard code
                product.Quantity = request.ProductPost.Quantity;
                product.InStock = request.ProductPost.InStock;
                product.ShowQuantity = request.ProductPost.ShowQuantity;
                product.HasShipping = request.ProductPost.HasShipping;
                product.Discount = request.ProductPost.Discount;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                #endregion

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "products");

                #region Main Image
                string mainImageFileName = Guid.NewGuid().ToString() + request.ProductPost.MainImage.FileName;

                using(FileStream fs = new FileStream(Path.Combine(path, mainImageFileName),FileMode.Create))
                {
                   await request.ProductPost.MainImage.CopyToAsync(fs);
                }


                var mainPhoto = new ProductPhoto();

                mainPhoto.ProductId = product.Id;
                mainPhoto.ImageURL = mainImageFileName;
                mainPhoto.IsMain = true;

                await _context.ProductPhotos.AddAsync(mainPhoto);
                await _context.SaveChangesAsync();
                #endregion

                #region Other Images
                foreach (var otherImage in request.ProductPost.OtherImages)
                {
                    string otherImageFileName = Guid.NewGuid().ToString() + otherImage.FileName;

                    using (FileStream fs = new FileStream(Path.Combine(path, otherImageFileName), FileMode.Create))
                    {
                        await otherImage.CopyToAsync(fs);
                    }


                    var otherPhoto = new ProductPhoto();

                    otherPhoto.ProductId = product.Id;
                    otherPhoto.ImageURL = mainImageFileName;
                    otherPhoto.IsMain = false;

                    await _context.ProductPhotos.AddAsync(otherPhoto);
                    await _context.SaveChangesAsync();
                }
                #endregion

                await _context.Database.CommitTransactionAsync();

                return RedirectToAction("List", "Product");

            }
            catch(Exception exp)
            {
                await _context.Database.RollbackTransactionAsync();
                return View(request);
            }
            
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
    }
}
