using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Context;
using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Core.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly ClinicDbContext _context;

        public VisitRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Visit>> ReadAllVisitsAsync()
        {
            return await _context.Visits.ToListAsync();
        }

        public async Task<IEnumerable<Visit>> ReadAllVisitsOfPatientByPatientIdAsync(Guid patientId)
        {
            return await _context.Visits.Where(v => v.PatientId == patientId).ToListAsync();
        }

        public async Task<Visit> ReadVisitByVisitIdAsync(Guid visitId)
        {
            return await _context.Visits.SingleOrDefaultAsync(v =>
                v.VisitId == visitId);
        }

        public async Task AddVisitAsync(Visit visit)
        {
            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVisitAsync(Visit visit)
        {
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitAsync(Visit visit)
        {
            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
        }

    }
}