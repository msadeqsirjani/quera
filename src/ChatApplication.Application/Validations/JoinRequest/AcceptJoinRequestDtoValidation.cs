using ChatApplication.Application.ViewModels.JoinRequest;
using FluentValidation;

namespace ChatApplication.Application.Validations.JoinRequest;

public class AcceptJoinRequestDtoValidation : AbstractValidator<AcceptJoinRequestDto>
{
    public AcceptJoinRequestDtoValidation()
    {
        RuleFor(x => x.JoinRequestId)
            .NotNull().WithMessage("JoinRequestId cannot be null");
    }
}