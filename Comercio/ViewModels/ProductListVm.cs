using Comercio.DTOs;

namespace Comercio.ViewModels
{
    public class ProductListVm
    {
        public List<ProductDto> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
