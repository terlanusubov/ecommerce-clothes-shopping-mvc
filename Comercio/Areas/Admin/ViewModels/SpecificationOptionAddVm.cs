using Comercio.DTOs;
using Comercio.Models;

namespace Comercio.Areas.Admin.ViewModels
{
    public class SpecificationOptionPostModel
    {
        public string Name  { get; set; }
        public int OptionGroupId { get; set; }
    }
    public class SpecificationOptionAddVm
    {
        public List<OptionGroupDto> OptionGroups { get; set; }
        public SpecificationOptionPostModel SpecificationOption { get; set; }
    }
}
