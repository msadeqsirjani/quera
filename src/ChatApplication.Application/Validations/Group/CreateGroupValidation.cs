using ChatApplication.Application.ViewModels.Group;
using FluentValidation;

namespace ChatApplication.Application.Validations.Group;

public class CreateGroupValidation : AbstractValidator<CreateGroupDto>
{
    public CreateGroupValidation()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name cannot be null")
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(255).WithMessage("Name is too long");
    }
}