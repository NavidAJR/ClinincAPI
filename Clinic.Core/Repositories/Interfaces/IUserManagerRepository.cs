using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Domain.Entities;

namespace Clinic.Core.Repositories.Interfaces
{
    public interface IUserManagerRepository
    {
        Task<IEnumerable<User>> ReadAllUsersAsync();
        Task<User> ReadUserByUserIdAsync(Guid userId);
        Task SoftDeleteUserByUserAsync(User user);
    }
}
