using Application.Common.Features.Auth.Commands;
using FluentValidation;


namespace Application.Common.Features.Auth.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.RegisterDto.FullName)
                .NotEmpty().WithMessage("Full name is required");

            RuleFor(x => x.RegisterDto.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^\d{10,15}$").WithMessage("Invalid phone number format");

            RuleFor(x => x.RegisterDto.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
