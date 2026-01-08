using Application.Common.Exceptions;
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
        private readonly IEmailService _emailService;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService, IEmailService emailService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
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

            if (!user.IsVerified)
            {
                throw new ConflictException("Too many failed login attempts, Please Request New OTP And Confirm it");
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPassword)
            {
                var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
                await _userManager.AccessFailedAsync(user);

                if (accessFailedCount >= 4) // This is the 5th failed attempt
                {
                    // Reload user to avoid concurrency exception (AccessFailedAsync updates the record)
                    var userToUpdate = await _userManager.FindByIdAsync(user.Id.ToString());
                    if (userToUpdate != null)
                    {
                        userToUpdate.IsVerified = false;
                        await _userManager.UpdateAsync(userToUpdate);
                    }

                    try
                    {
                        var emailRecipient = userToUpdate?.Email ?? user.Email;
                        if (!string.IsNullOrWhiteSpace(emailRecipient) && emailRecipient.Contains("@"))
                        {
                            await _emailService.SendEmailAsync(emailRecipient, "Security Alert: Too many failed login attempts", "there are many incorrect password");
                        }
                    }
                    catch
                    {
                        // Log email failure if logger was available, but don't crash the response
                    }

                    throw new ConflictException("Security Alert: Too many failed login attempts, Please Request New OTP");
                }
                return new AuthResponseDto { Success = false, Message = "Invalid credentials" };
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var token =await _jwtTokenService.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Success = true,
                Name = user.FullName,
                Message = "Login successful",
                Token = token,
                UserId = user.Id.ToString(),
                Email = user.Email,
                UserRole = user.UserRole
            };
        }
    }
}