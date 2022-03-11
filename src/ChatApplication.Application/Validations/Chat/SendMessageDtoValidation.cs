using ChatApplication.Application.ViewModels.Chat;
using FluentValidation;

namespace ChatApplication.Application.Validations.Chat;

public class SendMessageDtoValidation : AbstractValidator<SendMessageDto>
{
    public SendMessageDtoValidation()
    {
        RuleFor(x => x.Message)
            .NotNull().WithMessage("Message cannot be null")
            .NotEmpty().WithMessage("Message cannot be empty")
            .MaximumLength(4500).WithMessage("Message is too long");
    }
}