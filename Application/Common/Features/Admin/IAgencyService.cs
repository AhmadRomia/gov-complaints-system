using Application.Common.Features.Admin.Dtos;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Features.Admin
{
    public interface IAgencyService
        : IGenericService<GovernmentEntity, AgencyCreateDto, AgencyUpdateDto, AgencyDto, AgencyDto>
    {
    }
}
