using System;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Core.DTOs.PatientsDTOs
{
    public class PatientCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double? NationalCode { get; set; }

        [Required]
        public DateTime? Birthday { get; set; }
    }
}