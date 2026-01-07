using Application.Common.Features.Users.DTOs;
using Application.Common.Features.Users.Queries;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Features.Users.Handlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, PagingResult<UserProfileDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllUsersHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PagingResult<UserProfileDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _userManager.Users.AsQueryable();

            // Filter by Citizen role
            query = query.Where(u => u.UserRole == Domain.Enums.UserRoleEnum.Citizen);

            // Search functionality
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var search = request.SearchQuery.ToLower();
                query = query.Where(u => 
                    u.FullName.ToLower().Contains(search) || 
                    u.Email.ToLower().Contains(search) || 
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(search)));
            }

            // Get total count
            var totalCount = query.Count();

            // Apply pagination
            var users = query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .Select(user => new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsVerified = user.IsVerified,
                    Role = user.UserRole.ToString(),
                    IsActive = user.IsActive
                })
                .ToList();

            return new PagingResult<UserProfileDto>
            {
                Data = users,
                Count = totalCount
            };
        }
    }
}
