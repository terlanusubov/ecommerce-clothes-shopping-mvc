using Comercio.ServiceModels;
using Comercio.ViewModels;

namespace Comercio.Interfaces
{
    public interface IProductManager
    {
        Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request);
    }
}
