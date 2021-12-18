using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Domain.Entities
{
    public class Medicine
    {
        public Guid MedicineId { get; set; }

        [Required]
        public string Name { get; set; }

        #region Relations

        public virtual List<VisitsMedicines> VisitsMedicines { get; set; }

        #endregion
    }
}