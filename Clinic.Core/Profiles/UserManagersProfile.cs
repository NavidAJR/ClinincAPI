using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.UserManagersDTOs;
using Clinic.Domain.Entities;

namespace Clinic.Core.Profiles
{
    public class UserManagersProfile : Profile
    {
        public UserManagersProfile()
        {
            CreateMap<User, UserManagerReadUserDto>();
            CreateMap<UserManagerCreateUserDto, User>();
            CreateMap<UserManagerUpdateUserDto, User>()
                .ReverseMap();
        }
    }
}
