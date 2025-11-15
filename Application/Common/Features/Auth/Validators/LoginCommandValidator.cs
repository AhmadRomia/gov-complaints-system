using Application.Common.Features.Auth.Commands;
using FluentValidation;

namespace Application.Common.Features.Auth.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginDto.Email)
                .NotEmpty().WithMessage("Email or phone is required");

            RuleFor(x => x.LoginDto.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}