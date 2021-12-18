using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Clinic.Domain.Entities;

namespace Clinic.Core.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> ReadAllPatientsAsync();
        Task<Patient> ReadPatientByPatientIdAsync(Guid patientId);
        Task AddPatientAsync(Patient patient);
        Task<bool> IsNationalCodeExistsAsync(double? nationalCode);
        Task UpdatePatientAsync(Patient patient);
        Task DeletePatientAsync(Patient patient);
    }
}
