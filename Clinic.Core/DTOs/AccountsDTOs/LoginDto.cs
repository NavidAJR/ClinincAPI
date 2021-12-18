using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.AccountsDTOs
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}