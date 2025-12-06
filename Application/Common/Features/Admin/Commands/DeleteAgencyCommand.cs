using MediatR;

namespace Application.Common.Features.Admin.Commands
{
    public record DeleteAgencyCommand(Guid Id) : IRequest<bool>;
}
