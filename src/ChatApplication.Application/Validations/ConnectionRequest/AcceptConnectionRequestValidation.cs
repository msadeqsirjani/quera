using ChatApplication.Application.ViewModels.ConnectionRequest;
using FluentValidation;

namespace ChatApplication.Application.Validations.ConnectionRequest;

public class AcceptConnectionRequestValidation : AbstractValidator<AcceptConnectionRequestDto>
{
    public AcceptConnectionRequestValidation()
    {
        RuleFor(x => x.GroupId)
            .NotNull().WithMessage("GroupId cannot be null");
    }
}