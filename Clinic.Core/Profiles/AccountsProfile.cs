using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.AccountsDTOs;
using Clinic.Domain.Entities;

namespace Clinic.Core.Profiles
{
    internal class AccountsProfile : Profile
    {
        public AccountsProfile()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}
