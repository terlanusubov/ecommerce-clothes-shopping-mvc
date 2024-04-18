using Comercio.DTOs;

namespace Comercio.Areas.Admin.ViewModels
{
    public class SpecificationPostModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int IsSelected { get; set; }
    }
    public class SpecificationAddVm
    {
        public List<CategoryDto> Categories { get; set; }
        public SpecificationPostModel Specification { get; set; }
    }
}
