using Application.Common.Exceptions;
using Application.Common.Features.Users.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.Users.Handlers
{
    public class ActivateUserHandler : IRequestHandler<ActivateUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ActivateUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new BadRequestException("User not found");

            if (user.IsActive)
                throw new BadRequestException("User is already activated");


            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
