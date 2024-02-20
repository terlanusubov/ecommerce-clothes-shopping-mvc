namespace Comercio.Models
{
    public class Category : Entity<int>
    {
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int? Discount { get; set; }
    }
}
