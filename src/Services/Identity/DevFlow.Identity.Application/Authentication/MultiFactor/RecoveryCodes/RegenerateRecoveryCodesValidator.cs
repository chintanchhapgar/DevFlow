using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.RecoveryCodes;

internal sealed class RegenerateRecoveryCodesValidator
    : AbstractValidator<RegenerateRecoveryCodesCommand>
{
    public RegenerateRecoveryCodesValidator()
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
