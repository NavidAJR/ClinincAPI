using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Clinic.Domain.Entities;

namespace Clinic.Core.Repositories.Interfaces
{
    public interface IVisitRepository
    {
        Task<IEnumerable<Visit>> ReadAllVisitsAsync();
        Task<IEnumerable<Visit>> ReadAllVisitsOfPatientByPatientIdAsync(Guid patientId);
        Task<Visit> ReadVisitByVisitIdAsync(Guid visitId);
        Task AddVisitAsync(Visit visit);
        Task UpdateVisitAsync(Visit visit);
        Task DeleteVisitAsync(Visit visit);
    }
}
