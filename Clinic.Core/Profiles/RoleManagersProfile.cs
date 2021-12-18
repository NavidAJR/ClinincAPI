using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.RoleManagersDTOs;
using Clinic.Domain.Entities;

namespace Clinic.Core.Profiles
{
    public class RoleManagersProfile : Profile
    {
        public RoleManagersProfile()
        {
            CreateMap<Role, RoleManagerReadRoleDto>();
            CreateMap<RoleManagerCreateRoleDto, Role>();
            CreateMap<RoleManagerUpdateRoleDto, Role>();
        }
    }
}
