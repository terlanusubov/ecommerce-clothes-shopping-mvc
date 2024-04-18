namespace Comercio.Models
{
    public class ProductOption : Entity<int>
    {
        public Guid ProductId { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; }
    }
}
