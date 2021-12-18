using System;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.VisitsDTOs
{
    public class VisitCreateDto
    {
        public Guid PatientId { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }

        [Required]
        public DateTime? VisitedAt { get; set; }
    }
}