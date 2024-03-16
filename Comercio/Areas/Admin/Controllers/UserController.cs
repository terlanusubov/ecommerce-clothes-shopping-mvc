using Azure.Core;
using Comercio.Areas.Admin.DTOs;
using Comercio.Areas.Admin.Models;
using Comercio.Areas.Admin.ViewModels;
using Comercio.Data;
using Comercio.Enums;
using Comercio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin",AuthenticationSchemes = "AdminCookies")]
    public class UserController : BaseController
    {
        private ICompositeViewEngine _viewEngine;
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context,
            ICompositeViewEngine viewEngine)
        {
            _context = context;
            _viewEngine = viewEngine;       

        }
        public async Task<IActionResult> List(int page = 1)
        {
            var query = _context.Users.Where(u => true);

            var result = await SelectUsers(query, page);

            var vm = new UserListVm
            {
                CurrentPage = page,
                TotalPage = result.Item2,
                Users = result.Item1
            };

            return View(vm);
        }


        [HttpGet]
        public async Task<JsonResult> Filter([FromQuery]UserFilterModel request)
        {
            //TODO add validation

            var query = _context.Users.Where(u => 
                                            (String.IsNullOrEmpty(request.Name) || u.Name.ToLower().StartsWith(request.Name.ToLower())) &&
                                            (String.IsNullOrEmpty(request.Surname) || u.Surname.ToLower().StartsWith(request.Surname.ToLower())) &&
                                            (String.IsNullOrEmpty(request.Email) || u.Email.ToLower().StartsWith(request.Email.ToLower())));

            var result = await SelectUsers(query, request.Page);
            //TODO: deaktiv function

            var vm = new UserListVm
            {
                CurrentPage = request.Page,
                TotalPage = result.Item2,
                Users = result.Item1
            };

            var html = await RenderPartialViewToString("_UserListPartialView",vm);

            return Json(new
            {
                status = 200,
                data = html
            });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus([FromBody]ChangeUserStatusModel request)
        {
            //TODO: validation

            var user = await _context.Users.Where(u=> u.Id == request.UserId).FirstOrDefaultAsync();
            if(user is null)
            {
                return Json(new
                {
                    status = 400,
                    error = "Belə bir istifadəçi yoxdur."
                });
            }

            user.UserStatusId = request.StatusId;

            await _context.SaveChangesAsync();

            return Json(new
            {
                status = 200
            });

        }
     
        private async Task<(List<UserDto>,int)> SelectUsers(IQueryable<User> query, int page)
        {
            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)5);

            var users = await query.Include(u => u.UserRole)
                                 .Select(u => new UserDto
                                 {
                                     UserId = u.Id,
                                     Name = u.Name,
                                     Surname = u.Surname,
                                     Email = u.Email,
                                     Gender = u.Gender,
                                     GenderText = u.Gender != null ? ((bool)u.Gender ? "Kişi" : "Qadın") : "Qeyd olunmayıb",
                                     UserStatusId = u.UserStatusId,
                                     StatusText = u.UserStatusId == (byte)UserStatusEnum.Active ? "Aktiv" : "Deaktiv",
                                     Registered = u.Created,
                                     Role = u.UserRole.Name,
                                     RoleId = u.UserRoleId
                                 })
                                 .Skip((page - 1) * 5) //TODO: change hard code
                                 .Take(5)
                                 .ToListAsync();


            return (users,totalPage);
        }

        private async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.FindView(ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

    }
}
