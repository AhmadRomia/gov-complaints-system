using Application.Common.Features.Admin.Dtos;
using Application.Common.Features.Admin.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.Admin.Handlers
{
    public class GetAgencyUsersHandler : IRequestHandler<GetAgencyUsersQuery, List<AgencyUserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAgencyUsersHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<AgencyUserDto>> Handle(GetAgencyUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.Where(u => u.GovernmentEntityId != null);

            if (request.GovernmentEntityId.HasValue)
            {
                query = query.Where(u => u.GovernmentEntityId == request.GovernmentEntityId.Value);
            }

            var users = query.ToList();
            var result = new List<AgencyUserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Agency"))
                {
                    result.Add(new AgencyUserDto
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        GovernmentEntityId = user.GovernmentEntityId
                    });
                }
            }

            return result;
        }
    }
}
