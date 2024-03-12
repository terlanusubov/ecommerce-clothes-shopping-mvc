using Comercio.Areas.Admin.DTOs;

namespace Comercio.Areas.Admin.ViewModels
{
    public class UserListVm
    {
        public List<UserDto> Users { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
