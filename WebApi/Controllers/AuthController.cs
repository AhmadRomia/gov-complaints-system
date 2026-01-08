using Application.Common.Features.Auth.Commands;
using Application.Common.Features.Auth.DTOs;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _mediator.Send(new RegisterCommand(dto));
            return Ok(result);
        }
        [EnableRateLimiting("Fixed")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));
            return Ok(result);
        }

        [HttpPost("confirm-otp")]
        public async Task<IActionResult> ConfirmOtp(ConfirmOtpDto dto)
        {
            var result = await _mediator.Send(new ConfirmOtpCommand(dto));
            return Ok(result);
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(ResendOtpDto dto)
        {
            var result = await _mediator.Send(new ResendOtpCommand(dto));
            return Ok(result);
        }
    }
}
