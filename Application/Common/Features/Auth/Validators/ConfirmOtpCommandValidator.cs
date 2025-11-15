using Application.Common.Features.Auth.Commands;
using FluentValidation;

namespace Application.Common.Features.Auth.Validators
{
    public class ConfirmOtpCommandValidator : AbstractValidator<ConfirmOtpCommand>
    {
        public ConfirmOtpCommandValidator()
        {
            RuleFor(x => x.ConfirmOtpDto.Identifier)
                .NotEmpty().WithMessage("Identifier (email or phone) is required");

            RuleFor(x => x.ConfirmOtpDto.Code)
                .NotEmpty().WithMessage("OTP code is required")
                .Length(6).WithMessage("OTP code must be 6 digits")
                .Matches("^\\d{6}$").WithMessage("OTP code must be numeric");
        }
    }
}