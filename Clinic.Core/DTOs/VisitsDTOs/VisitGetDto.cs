using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTOs.VisitsDTOs
{
    public class VisitGetDto
    {
        public string PatientName { get; set; }

        public DateTime CreatedAt { get; set; }

        
        public DateTime VisitedAt { get; set; }
    }
}
