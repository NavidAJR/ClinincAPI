using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Context;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Core.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ClinicDbContext _context;

        public TokenRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdentityUserRole<Guid>>> ReadAllUserRolesAsync(User user)
        {
            return await _context.UserRoles.Where(ur =>
                ur.UserId == user.Id).ToListAsync();
        }
    }
}