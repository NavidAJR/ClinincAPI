using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.Core.DTOs.PatientsDTOs;
using Clinic.Domain.Entities;

namespace Clinic.Core.Profiles
{
    public class PatientsProfile : Profile
    {
        public PatientsProfile()
        {
            CreateMap<Patient, PatientGetDto>();
            CreateMap<PatientCreateDto, Patient>();
            CreateMap<PatientUpdateDto, Patient>()
                .ReverseMap();
        }
    }
}
