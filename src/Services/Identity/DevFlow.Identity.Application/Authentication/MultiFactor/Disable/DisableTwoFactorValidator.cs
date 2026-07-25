using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Disable;

internal sealed class DisableTwoFactorValidator
    : AbstractValidator<DisableTwoFactorCommand>
{
    public DisableTwoFactorValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.IsRecoveryCode)
            .NotNull();
    }
}
