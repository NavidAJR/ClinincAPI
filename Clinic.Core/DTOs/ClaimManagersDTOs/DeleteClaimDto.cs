using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.ClaimManagersDTOs
{
    public class DeleteClaimDto
    {
        [Required]
        public string ClaimType { get; set; }
    }
}