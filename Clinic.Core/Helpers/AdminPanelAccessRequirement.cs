using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Clinic.Core.Helpers
{
    public class AdminPanelAccessRequirement : IAuthorizationRequirement
    {
        public string Claim { get; set; }

        public AdminPanelAccessRequirement(string claim)
        {
            Claim = claim;
        }
    }

    public class AdminPanelAccessHandler : AuthorizationHandler<AdminPanelAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminPanelAccessRequirement requirement)
        {
            var claims = context.User.Claims.ToList();

            if (claims.Any(c=> c.Type.ToLower() == "websitestaff"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
