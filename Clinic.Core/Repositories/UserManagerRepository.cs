using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Core.Repositories.Interfaces;
using Clinic.Domain.Context;
using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Core.Repositories
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly ClinicDbContext _context;

        public UserManagerRepository(ClinicDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> ReadUserByUserIdAsync(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task SoftDeleteUserByUserAsync(User user)
        {
            user.IsDelete = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
