namespace Comercio.Models
{
    public class ProductVariant : Entity<int>
    {
        public string Name { get; set; }

        public ProductVariant()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
}
