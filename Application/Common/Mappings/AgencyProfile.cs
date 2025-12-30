using Application.Common.Features.Admin.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class AgencyProfile : Profile
    {
        public AgencyProfile()
        {
            CreateMap<GovernmentEntity, AgencyDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
            CreateMap<AgencyCreateDto, GovernmentEntity>()
                .ForMember(d => d.LogoUrl, opt => opt.MapFrom(s => s.LogoUrl));
            CreateMap<AgencyUpdateDto, GovernmentEntity>();
        }
    }
}
