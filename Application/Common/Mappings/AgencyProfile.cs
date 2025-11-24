using Application.Common.Features.Admin.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<GovernmentEntity, AgencyDto>();
            CreateMap<AgencyCreateDto, GovernmentEntity>();
            CreateMap<AgencyUpdateDto, GovernmentEntity>();
        }
    }
}
