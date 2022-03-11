using ChatApplication.Application.ViewModels.Authentication;
using FluentValidation;

namespace ChatApplication.Application.Validators.Authentication;

public class LoginDtoValidation : AbstractValidator<LoginDto>
{
    public LoginDtoValidation()
    {
        RuleFor(x => x.Email)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(x => x.Password)
            .NotNull().WithMessage("Password cannot be null")
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}