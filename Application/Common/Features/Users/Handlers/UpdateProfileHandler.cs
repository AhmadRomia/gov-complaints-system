using Application.Common.Features.Users.Commands;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Features.Users.Handlers
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateProfileHandler(ICurrentUserService currentUser, UserManager<ApplicationUser> userManager)
        {
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId.ToString());
            if (user == null) throw new UnauthorizedAccessException();

            user.FullName = request.Dto.FullName ?? user.FullName;
            user.PhoneNumber = request.Dto.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
