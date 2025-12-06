using MediatR;

namespace Application.Common.Features.Users.Commands
{
    public class ActivateUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }

        public ActivateUserCommand(Guid id)
        {
            UserId = id;
        }
    }
}
