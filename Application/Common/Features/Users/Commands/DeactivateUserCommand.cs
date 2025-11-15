using MediatR;

namespace Application.Common.Features.Users.Commands
{
    public class DeactivateUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }

        public DeactivateUserCommand(Guid id)
        {
            UserId = id;
        }
    }
}
