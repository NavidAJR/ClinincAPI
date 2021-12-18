using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.RoleManagersDTOs
{
    public class RoleManagerCreateRoleDto
    {
        [Required]
        public string Name { get; set; }
    }
}