using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Domain.Entities
{
    public class Visit
    {
        public Guid VisitId { get; set; }
        public Guid PatientId { get; set; }

        public Guid CreatorId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime VisitedAt { get; set; }



        #region Relations
        public virtual Patient Patients { get; set; }

        [ForeignKey("CreatorId")]
        public virtual User User { get; set; }

        public virtual List<VisitsMedicines> VisitsMedicines { get; set; }
        #endregion
    }
}