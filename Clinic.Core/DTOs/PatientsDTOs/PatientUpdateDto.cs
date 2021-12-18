using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Clinic.Core.DTOs.PatientsDTOs
{
    public class PatientUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double? NationalCode { get; set; }

        [Required]
        public DateTime? Birthday { get; set; }
    }
}