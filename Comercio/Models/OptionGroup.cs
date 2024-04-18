namespace Comercio.Models
{
    public class OptionGroup:Entity<int>
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Option> Options { get; set; }
    }
}
