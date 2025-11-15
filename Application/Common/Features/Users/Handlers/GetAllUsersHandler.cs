using Application.Common.Features.Users.DTOs;
using Application.Common.Features.Users.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Features.Users.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserProfileDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserProfileDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.ToList();

            var result = new List<UserProfileDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsVerified = user.IsVerified,
                    Role = roles.FirstOrDefault()
                });
            }

            return result;
        }
    }
}
