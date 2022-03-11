using ChatApplication.Application.ViewModels.ConnectionRequest;
using FluentValidation;

namespace ChatApplication.Application.Validations.ConnectionRequest;

public class SendConnectionRequestValidation : AbstractValidator<SendConnectionRequestDto>
{
    public SendConnectionRequestValidation()
    {
        RuleFor(x => x.GroupId)
            .NotNull().WithMessage("GroupId cannot be null");
    }
}