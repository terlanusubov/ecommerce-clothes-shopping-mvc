using Comercio.Areas.Admin.DTOs;
using Comercio.Areas.Admin.ViewModels;
using Comercio.Data;
using Comercio.DTOs;
using Comercio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminCookies")]
    public class SpecificationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SpecificationController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _context.Categories.Select(_ => new CategoryDto
            {
                Name = _.Name,
                Slogan = _.Slogan ?? "",
                CategoryId = _.Id,
                BackgroundImageUrl = _.BackgroundImageURL ?? "",
                Priority = _.Priority ?? 0,
                ParentId = _.ParentId

            }).ToListAsync();

            var vm = new SpecificationAddVm
            {
                Categories = categories
            };


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecificationAddVm request)
        {
            //TODO: Validation
            //TODO: transaction

            OptionGroup group = new OptionGroup();

            group.CategoryId = request.Specification.CategoryId;
            group.Name = request.Specification.Name;


            await _context.OptionGroups.AddAsync(group);
            await _context.SaveChangesAsync();

            if(request.Specification.IsSelected != 1)
            {
                Comercio.Models.Option defaultOption = new Comercio.Models.Option();

                defaultOption.Name = request.Specification.Name;
                defaultOption.IsSelected = false;
                defaultOption.OptionGroupId = group.Id;

                await _context.Options.AddAsync(defaultOption);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("List", "Specification");
        }

        [HttpGet]
        public async Task<IActionResult> AddOptionToSpecification()
        {
            var optionGroups = await _context.OptionGroups
                .Include(_ => _.Options)
                .Where(_ => !_.Options.Any(c=>c.Name == _.Name)).Select(_ => new OptionGroupDto
            {
                Name = _.Name,
                OptionGroupId = _.Id

            }).ToListAsync();

            var vm = new SpecificationOptionAddVm
            {
                OptionGroups = optionGroups
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddOptionToSpecification(SpecificationOptionAddVm request)
        {
            //TODO: validation
            //TODO: transaction

            Option option = new Option();
            option.OptionGroupId = request.SpecificationOption.OptionGroupId;
            option.Name = request.SpecificationOption.Name;
            option.IsSelected = true;

            await _context.Options.AddAsync(option);
            await _context.SaveChangesAsync();

            return RedirectToAction("List", "Specification");
        }

        [HttpGet]
        public async Task<JsonResult> GetSpecificationsByCategoryId(int? categoryId)
        {
            if(categoryId is null)
            {
                return Json(new
                {
                    status = 400
                });
            }

            var specifications = await _context.OptionGroups.Include(_ => _.Options)
                                                            .Where(_ => _.CategoryId == categoryId)
                                                            .Select(_ => new SpecificationDto
                                                            {
                                                                SpecId = _.Id,
                                                                Name = _.Name,
                                                                Options = _.Options == null ? null : _.Options.Select(o => new SpecOptionDto
                                                                {
                                                                    SpecOptionId = o.Id,
                                                                    Name = o.Name
                                                                }).ToList()
                                                            }).ToListAsync();

            return Json(new
            {
                status = 200,
                data = specifications
            });
        }
    }
}
