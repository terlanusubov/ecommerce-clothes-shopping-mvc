namespace Comercio.Models
{
    public class Option : Entity<int>
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public int OptionGroupId { get; set; }

        public OptionGroup OptionGroup { get; set; }
    }
}
