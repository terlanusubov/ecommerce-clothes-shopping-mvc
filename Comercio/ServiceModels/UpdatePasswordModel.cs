using System.ComponentModel.DataAnnotations;

namespace Comercio.ServiceModels
{
    public class UpdatePasswordModel
    {
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }
}
