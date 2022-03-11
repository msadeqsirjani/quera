using ChatApplication.Application.ViewModels.Authentication;
using FluentValidation;

namespace ChatApplication.Application.Validators.Authentication;

public class SignInDtoValidation : AbstractValidator<SignInDto>
{
    public SignInDtoValidation()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(255).WithMessage("Name is too long");

        RuleFor(x=>x.Email)
            .NotNull().WithMessage("Email cannot be null")
            .NotEmpty().WithMessage("Email cannot be empty")
            .MaximumLength(255).WithMessage("Email is too long");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password cannot be null")
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}