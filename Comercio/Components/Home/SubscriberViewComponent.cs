using Comercio.Data;
using Microsoft.AspNetCore.Mvc;

namespace Comercio.Components.Home
{
    public class SubscriberViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public SubscriberViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
