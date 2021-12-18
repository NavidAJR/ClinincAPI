using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Domain.Entities
{
    public class Patient
    {
        public Guid PatientId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double NationalCode { get; set; }

        [Required]
        public DateTime Birthday { get; set; }


        #region Relations

        public virtual List<Visit> Visits { get; set; }


        #endregion
    }
}
