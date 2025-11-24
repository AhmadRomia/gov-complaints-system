using Application.Common.Features.Users.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Features.Users.Handlers
{
    public class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeactivateUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null) return false;

            user.IsActive = false; 
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
