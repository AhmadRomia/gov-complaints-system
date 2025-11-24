using Application.Common.Features.Auth.DTOs;
using MediatR;

namespace Application.Common.Features.Auth.Commands
{
    public record ResendOtpCommand(ResendOtpDto ResendOtpDto) : IRequest<AuthResultDto>;
}
