using AutoMapper;
using Clinic.Core.DTOs.VisitsDTOs;
using Clinic.Domain.Entities;

namespace Clinic.Core.Profiles
{
    public class VisitsProfile : Profile
    {
        public VisitsProfile()
        {
            CreateMap<Visit, VisitGetDto>()
                .ReverseMap();
            CreateMap<VisitCreateDto, Visit>();
            CreateMap<Visit, VisitUpdateDto>()
                .ReverseMap();
        }
    }
}