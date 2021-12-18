using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.RoleManagersDTOs
{
    public class RoleManagerUpdateRoleDto
    {
        [Required]
        public string Name { get; set; }
    }
}