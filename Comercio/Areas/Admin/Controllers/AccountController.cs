using Comercio.Areas.Admin.Models;
using Comercio.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Comercio.Data;

namespace Comercio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginModel request)
        {
            if (!ModelState.IsValid) { return View(request); }

            var user = await _context.Users
                                       .Include(c => c.UserRole)
                                       .Where(c => c.Email == request.Email &&
                                                      c.UserStatusId == (int)UserStatusEnum.Active &&
                                                      c.UserRoleId == (byte)UserRoleEnum.Admin).FirstOrDefaultAsync();

            if (user is null)
            {
                ModelState.AddModelError("", "Email or password is not correct");
                return View(request);
            }

            var result = user.CheckPassword(request.Password);

            if (!result)
            {
                ModelState.AddModelError("", "Email or password is not correct");
                return View(request);
            }

            var claims = new List<Claim>
              {
                  new Claim("Name", user.Name),
                  new Claim("Surname", user.Surname),
                  new Claim("Email", user.Email),
                  new Claim("Id", user.Id.ToString()),
                  new Claim("RoleId", user.UserRoleId.ToString()),
                  new Claim(ClaimTypes.Role, user.UserRole.Name)
              };

            var claimsIdentity = new ClaimsIdentity(
                claims, "AdminCookies");


            await HttpContext.SignInAsync(
               "AdminCookies",
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
    }
}
