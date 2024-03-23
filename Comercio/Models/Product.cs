namespace Comercio.Models
{
    /// <summary>
    /// Products
    /// </summary>
    public class Product:Entity<Guid>
    {
        public int ProductVariantId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Barcode { get; set; }
        public int CategoryId { get; set; }
        public byte GenderTypeId { get; set; }
        public string Description { get; set; }
        public double SellAmount { get; set; }
        public double BuyAmount { get; set; }
        public int BuyLimit { get; set; }
        public int Quantity { get; set; }
        public bool InStock { get; set; }
        public int? ShowQuantity { get; set; }
        public bool HasShipping { get; set; }
        public int? Discount { get; set; }

        public ProductVariant ProductVariant { get; set; }
        public Category Category { get; set; }
        public GenderType GenderType { get; set; }
        public ICollection<ProductOption> ProductOptions { get; set; }
        public ICollection<ProductPhoto> ProductPhotos { get; set; }
        public ICollection<UserWishlist> UserWishlists { get; set; }
    }
}
