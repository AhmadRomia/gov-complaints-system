

using Application.Common.Features.ComplsintUseCase.DTOs;
using FluentValidation;

namespace Application.Common.Features.ComplsintUseCase.Validators
{
    public class ComplaintCreateDtoValidator : AbstractValidator<ComplaintCreateDto>
    {
        public ComplaintCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.Severity)
                .InclusiveBetween(1, 5).WithMessage("Severity must be between 1 and 5");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid complaint type");

            RuleFor(x => x.GovernmentEntityId)
                .NotNull().WithMessage("Government entity is required");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required")
                .MaximumLength(300).WithMessage("Location must not exceed 300 characters");
        }
    }
}
