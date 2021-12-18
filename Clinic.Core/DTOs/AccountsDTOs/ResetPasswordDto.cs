using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.AccountsDTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}