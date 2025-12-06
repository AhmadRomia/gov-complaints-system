using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Commands
{
    public record UpdateAgencyCommand( AgencyUpdateDto Dto) : IRequest<bool>;
}
