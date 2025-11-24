using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Commands
{
    public record CreateAgencyCommand(AgencyCreateDto Dto) : IRequest<AgencyDto>;
}
