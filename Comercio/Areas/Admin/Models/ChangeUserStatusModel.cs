namespace Comercio.Areas.Admin.Models
{
    public class ChangeUserStatusModel
    {
        public Guid UserId { get; set; }
        public byte StatusId { get; set; }
    }
}
