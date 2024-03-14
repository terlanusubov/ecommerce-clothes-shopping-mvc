namespace Comercio.Areas.Admin.Models
{
    public class UserFilterModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Page { get; set; } = 1;
    }
}
