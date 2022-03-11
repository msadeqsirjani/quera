using ChatApplication.Application.ViewModels.JoinRequest;
using FluentValidation;

namespace ChatApplication.Application.Validations.JoinRequest;

public class SendJoinRequestDtoValidation : AbstractValidator<SendJoinRequestDto>
{
    public SendJoinRequestDtoValidation()
    {
        RuleFor(x => x.GroupId)
            .NotNull().WithMessage("GroupId cannot be null");
    }
}