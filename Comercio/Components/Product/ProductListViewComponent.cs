using Comercio.Data;
using Comercio.DTOs;
using Comercio.Interfaces;
using Comercio.ServiceModels;
using Comercio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comercio.Components.Product
{
    public class ProductListViewComponent : ViewComponent
    {
        private readonly IProductManager _productManager;

        private readonly IConfiguration _configuration;
        public ProductListViewComponent(IConfiguration configuration, IProductManager productManager)
        {
            _configuration = configuration;
            _productManager = productManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(ProductListQueryModel request)
        {
            var vm = await _productManager.GetFilteredProducts(request);

            return View(vm);

        }
    }
}
