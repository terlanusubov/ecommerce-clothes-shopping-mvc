using Comercio.Areas.Admin.ViewModels;
using Comercio.DTOs;
using Comercio.ServiceModels;
using Comercio.ViewModels;

namespace Comercio.Interfaces
{
    public interface IProductManager
    {
        Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request);

        Task<(bool,Dictionary<string,string>)> ValidateProduct(ProductPostModel request);

        Task<bool> CreateProduct(ProductPostModel request);

        Task<ProductDto> GetProductById(Guid productId);
    }
}
