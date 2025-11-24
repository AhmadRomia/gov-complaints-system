using Application.Common.Features.Auth.DTOs;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwt;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwt)
        {
            _userManager = userManager;
            _jwt = jwt;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return new AuthResponseDto
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                Token =await _jwt.GenerateTokenAsync(user)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid password");

            return new AuthResponseDto
            {
                UserId = user.Id.ToString(),
                Email = user.Email,
                Token =await _jwt.GenerateTokenAsync(user)
            };
        }
    }
}
