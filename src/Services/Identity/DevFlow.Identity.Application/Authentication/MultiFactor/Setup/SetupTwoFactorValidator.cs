using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Setup;

internal sealed class SetupTwoFactorValidator
    : AbstractValidator<SetupTwoFactorCommand>
{
    public SetupTwoFactorValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
