namespace Comercio.Models
{
    public class Option : Entity<int>
    {
        public string Name { get; set; }
        public int OptionGroupId { get; set; }
    }
}
