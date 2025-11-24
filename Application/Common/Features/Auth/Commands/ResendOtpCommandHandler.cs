using Application.Common.Features.Auth.DTOs;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Features.Auth.Commands
{
    public class ResendOtpCommandHandler : IRequestHandler<ResendOtpCommand, AuthResultDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;

        public ResendOtpCommandHandler(UserManager<ApplicationUser> userManager, IOtpService otpService)
        {
            _userManager = userManager;
            _otpService = otpService;
        }

        public async Task<AuthResultDto> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            var identifier = request.ResendOtpDto.Identifier?.Trim();
            if (string.IsNullOrEmpty(identifier))
                return new AuthResultDto { Success = false, Message = "Identifier is required" };

            ApplicationUser? user = await _userManager.FindByEmailAsync(identifier);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(identifier); 
            }

            if (user == null)
                return new AuthResultDto { Success = false, Message = "User not found" };

            if (user.IsVerified)
                return new AuthResultDto { Success = false, Message = "User already verified" };

            await _otpService.GenerateOtpAsync(user);

            return new AuthResultDto
            {
                Success = true,
                Message = "OTP resent successfully"
            };
        }
    }
}
