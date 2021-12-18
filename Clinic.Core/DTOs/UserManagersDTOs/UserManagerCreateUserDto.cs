using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.UserManagersDTOs
{
    public class UserManagerCreateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}