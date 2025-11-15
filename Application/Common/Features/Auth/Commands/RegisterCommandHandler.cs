using Application.Common.Features.Auth.DTOs;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Common.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResultDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOtpService _otpService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IOtpService otpService,
            IMapper mapper)
        {
            _userManager = userManager;
            _otpService = otpService;
            _mapper = mapper;
        }

        public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RegisterDto;

            var exists = await _userManager.FindByNameAsync(dto.Phone);
            if (exists != null)
                return new AuthResultDto { Success = false, Message = "User already exists" };

            var user = _mapper.Map<ApplicationUser>(dto);
            user.UserName = dto.Phone;
            user.PhoneNumber = dto.Phone;
            user.IsVerified = false;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Message = string.Join(" | ", result.Errors.Select(e => e.Description))
                };
            }

            await _otpService.GenerateOtpAsync(user);

            await _userManager.AddToRoleAsync(user, "Citizen");

            return new AuthResultDto
            {
                Success = true,
                Message = "User registered successfully. OTP sent."
            };
        }
    }
}
