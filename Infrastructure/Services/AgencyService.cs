using Application.Common.Features.Admin;
using Application.Common.Features.Admin.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class AgencyService
        : GenericService<GovernmentEntity, AgencyCreateDto, AgencyUpdateDto, AgencyDto, AgencyDto>, IAgencyService
    {
        public AgencyService(IRepository<GovernmentEntity> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
