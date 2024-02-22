namespace Comercio.Models
{
    public class ProductPhoto : Entity<int>
    {
        public Guid ProductId { get; set; }
        public string ImageURL { get; set; }
        public bool IsMain { get; set; }

        public Product Product { get; set; }
    }
}
