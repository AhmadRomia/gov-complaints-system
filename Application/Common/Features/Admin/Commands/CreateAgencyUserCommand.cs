using Application.Common.Features.Admin.Dtos;
using MediatR;

namespace Application.Common.Features.Admin.Commands
{
    public class CreateAgencyUserCommand : IRequest<AgencyUserDto>
    {
        public AgencyUserCreateDto Dto { get; }

        public CreateAgencyUserCommand(AgencyUserCreateDto dto)
        {
            Dto = dto;
        }
    }
}
