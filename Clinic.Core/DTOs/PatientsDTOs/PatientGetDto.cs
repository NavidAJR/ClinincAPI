using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.DTOs.VisitsDTOs;
using Clinic.Domain.Entities;

namespace Clinic.Core.DTOs.PatientsDTOs
{
    public class PatientGetDto
    {
        public Guid PatientId { get; set; }
        public string Name { get; set; }

        public double NationalCode { get; set; }

        public DateTime Birthday { get; set; }
    }
}
