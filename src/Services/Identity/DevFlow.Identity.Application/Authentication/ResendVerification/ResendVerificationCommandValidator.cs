using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.ResendVerification;

internal sealed class ResendVerificationCommandValidator
    : AbstractValidator<ResendVerificationCommand>
{
    public ResendVerificationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}
