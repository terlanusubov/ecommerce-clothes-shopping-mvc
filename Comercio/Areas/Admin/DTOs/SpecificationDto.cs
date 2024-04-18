namespace Comercio.Areas.Admin.DTOs
{
    public class SpecOptionDto
    {
        public int SpecOptionId { get; set; }
        public string Name { get; set; }
    }
    public class SpecificationDto
    {
        public int SpecId { get; set; }
        public string Name { get; set; }
        public List<SpecOptionDto> Options { get; set; }
    }
}
