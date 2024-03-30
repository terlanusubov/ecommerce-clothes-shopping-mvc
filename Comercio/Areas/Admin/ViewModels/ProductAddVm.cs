using Comercio.DTOs;

namespace Comercio.Areas.Admin.ViewModels
{
    public class ProductGetModel
    {
        public List<CategoryDto> Categories { get; set; }

        public List<GenderTypeDto> Genders { get; set; }
    }
    public class ProductPostModel
    {
        public int ProductVariantId { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public int CategoryId { get; set; }
        public byte GenderTypeId { get; set; }
        public string Description { get; set; }
        public double SellAmount { get; set; }
        public double BuyAmount { get; set; }
        public int? BuyLimit { get; set; }
        public int Quantity { get; set; }
        public bool InStock { get; set; }
        public int? ShowQuantity { get; set; }
        public bool HasShipping { get; set; }
        public int? Discount { get; set; }
        public IFormFile MainImage { get; set; } 
        public List<IFormFile> OtherImages { get; set; }
    }
    public class ProductAddVm
    {
        public ProductGetModel ProductGet { get; set; }
        public ProductPostModel ProductPost { get; set; }
    }
}
