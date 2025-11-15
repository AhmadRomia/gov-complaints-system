using Application.Common.Features.Users.DTOs;
using MediatR;


namespace Application.Common.Features.Users.Commands
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public UpdateProfileDto Dto { get; set; }

        public UpdateProfileCommand(UpdateProfileDto dto)
        {
            Dto = dto;
        }
    }
}
