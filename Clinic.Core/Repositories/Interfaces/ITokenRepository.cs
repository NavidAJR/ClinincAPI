using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Clinic.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Clinic.Core.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        Task<IEnumerable<IdentityUserRole<Guid>>> ReadAllUserRolesAsync(User user);
    }
}
