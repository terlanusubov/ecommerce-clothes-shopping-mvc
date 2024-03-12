namespace Comercio.Areas.Admin.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime? Registered { get; set; }
        public int UserStatusId { get; set; }
        public string StatusText { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public string GenderText { get; set; }
        public bool? Gender { get; set; }
    }
}
