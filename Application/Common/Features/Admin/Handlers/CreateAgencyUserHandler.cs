using Application.Common.Features.Admin.Commands;
using Application.Common.Features.Admin.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.Admin.Handlers
{
    public class CreateAgencyUserHandler : IRequestHandler<CreateAgencyUserCommand, AgencyUserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateAgencyUserHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AgencyUserDto> Handle(CreateAgencyUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email,
                GovernmentEntityId = dto.GovernmentEntityId,
                IsVerified = true,
                IsActive = true,
                UserRole = Domain.Enums.UserRoleEnum.Agency
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Agency");

            return new AgencyUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                GovernmentEntityId = user.GovernmentEntityId,
                IsActive = user.IsActive
            };
        }
    }
}
