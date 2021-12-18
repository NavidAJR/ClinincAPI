using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.UserManagersDTOs
{
    public class UserManagerUpdateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

    }
}