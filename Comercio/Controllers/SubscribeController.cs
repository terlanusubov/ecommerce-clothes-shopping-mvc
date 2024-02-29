using Comercio.Data;
using Comercio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscribeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Email(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                TempData["error"] = "E-poçt boş ola bilməz.";
                return RedirectToAction("Index", "Home");
            }

            var subscriber = await _context
                                    .Subscribers
                                        .Where(c => c.Email == email)
                                            .FirstOrDefaultAsync();

            if(subscriber is not null)
            {
                TempData["error"] = "E-poçt artıq abune olub.";
                return RedirectToAction("Index", "Home");
            }

            subscriber = new Subscriber();

            subscriber.Email = email;

            subscriber.Created = DateTime.Now;

            subscriber.Updated = DateTime.Now;

            await _context.Subscribers.AddAsync(subscriber);

            await _context.SaveChangesAsync();

            TempData["success"] = "E-poçt uğurla abune oldu";

            return RedirectToAction("Index", "Home");
        }
    }
}
