using System.ComponentModel.DataAnnotations;

namespace Comercio.Areas.Admin.Models
{
    public class AdminLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
