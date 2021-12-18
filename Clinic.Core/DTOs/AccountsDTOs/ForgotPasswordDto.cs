using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.AccountsDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        public string Email { get; set; }
    }
}