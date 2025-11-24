using Application.Common.Features.Auth.DTOs;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.Auth.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LoginDto;

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(dto.Email); 
            }

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "User not found" };
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPassword)
            {
                return new AuthResponseDto { Success = false, Message = "Invalid credentials" };
            }

            if (!user.IsVerified)
            {
                return new AuthResponseDto { Success = false, Message = "User not verified" };
            }

            var token =await _jwtTokenService.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                UserId = user.Id.ToString(),
                Email = user.Email
            };
        }
    }
}