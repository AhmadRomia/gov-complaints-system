using Application.Common.Features.Users.DTOs;
using Application.Common.Features.Users.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.Users.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserProfileDto?>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByIdHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsVerified = user.IsVerified,
                Role = roles.FirstOrDefault(),
                IsActive = user.IsActive
            };
        }
    }
}
