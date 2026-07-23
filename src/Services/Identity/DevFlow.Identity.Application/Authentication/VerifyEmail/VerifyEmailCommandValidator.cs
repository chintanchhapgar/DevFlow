using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

internal sealed class VerifyEmailCommandValidator
    : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .MaximumLength(512);
    }
}
