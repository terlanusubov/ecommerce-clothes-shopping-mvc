namespace Comercio.Models
{
    public class ProductOption : Entity<int>
    {
        public Guid ProductId { get; set; }
        public int OptionId { get; set; }
        public int OptionGroupId { get; set; }
    }
}
