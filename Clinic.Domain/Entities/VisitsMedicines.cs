using System;

namespace Clinic.Domain.Entities
{
    public class VisitsMedicines
    {
        public Guid VisitsMedicinesId { get; set; }

        public Guid VisitId { get; set; }
        public Guid MedicineId { get; set; }

        #region Relations

        public virtual Visit Visit { get; set; }
        public virtual Medicine Medicine { get; set; }

        #endregion
    }
}