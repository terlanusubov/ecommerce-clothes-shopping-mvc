namespace Comercio.Models
{
    public class Category : Entity<int>
    {
        public string Name { get; set; }
        public string? BackgroundImageURL { get; set; }
        public string? Slogan { get; set; }
        public int? Priority { get; set; }
        public bool IsMainPage { get; set; }
        public int? ParentId { get; set; }
        public int? Discount { get; set; }
    }
}
