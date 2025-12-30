using Application.Common.Features.Auth.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.Common.Interfaces;

namespace Application.Common.Features.Auth.Commands
{
    public class ConfirmOtpCommandHandler : IRequestHandler<ConfirmOtpCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;

        public ConfirmOtpCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> Handle(ConfirmOtpCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ConfirmOtpDto;
            var identifier = dto.Identifier?.Trim();
            var code = dto.Code?.Trim();

            ApplicationUser? user = null;

            if (!string.IsNullOrEmpty(identifier))
            {
                user = await _userManager.FindByEmailAsync(identifier);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(identifier); 
                }
            }

            if (user == null)
            {
                return new AuthResponseDto { Success = false, Message = "User not found" };
            }

            if (user.IsVerified)
            {
                return new AuthResponseDto { Success = false, Message = "User already verified" };
            }

            if (string.IsNullOrEmpty(user.OtpCode) || user.OtpExpiresAt == null)
            {
                return new AuthResponseDto { Success = false, Message = "No OTP pending" };
            }

            if (user.OtpExpiresAt < DateTime.UtcNow)
            {
                return new AuthResponseDto { Success = false, Message = "OTP expired" };
            }

            if (!string.Equals(user.OtpCode, code, StringComparison.Ordinal))
            {
                return new AuthResponseDto { Success = false, Message = "Invalid OTP code" };
            }

                
            user.IsVerified = true;
            user.OtpCode = null;
            user.OtpExpiresAt = null;
            await _userManager.UpdateAsync(user);

            var token =await _jwtTokenService.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Message = "OTP confirmed successfully",
                Token = token,
                UserId = user.Id.ToString(),
                Email = user.Email,
                UserRole = user.UserRole,
            };
        }
    }
}