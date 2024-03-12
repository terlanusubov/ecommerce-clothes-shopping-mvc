using Comercio.Areas.Admin.DTOs;
using Comercio.Areas.Admin.ViewModels;
using Comercio.Data;
using Comercio.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin",AuthenticationSchemes = "AdminCookies")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List(int page = 1)
        {
            var query = _context.Users.Where(u => true);

            query = query.OrderByDescending(u => u.Created);

            var count = await query.CountAsync();

            var totalPage = (int)Math.Ceiling(count / (decimal)50);

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
                                 .Skip((page - 1) * 50) //TODO: change hard code
                                 .Take(50)
                                 .ToListAsync();

            var vm = new UserListVm
            {
                CurrentPage = page,
                TotalPage = totalPage,
                Users = users
            };

            return View(vm);
        }
    }
}
