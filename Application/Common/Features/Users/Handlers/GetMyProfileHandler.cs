using Application.Common.Features.Users.DTOs;
using Application.Common.Features.Users.Queries;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Features.Users.Handlers
{
    public class GetMyProfileHandler : IRequestHandler<GetMyProfileQuery, UserProfileDto>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetMyProfileHandler(ICurrentUserService currentUser, UserManager<ApplicationUser> userManager)
        {
            _currentUser = currentUser;
            _userManager = userManager;
        }

        public async Task<UserProfileDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated)
                throw new UnauthorizedAccessException();

            var user = await _userManager.FindByIdAsync(_currentUser.UserId.ToString());
            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsVerified = user.IsVerified,
                Role = roles.FirstOrDefault()
            };
        }
    }
}
