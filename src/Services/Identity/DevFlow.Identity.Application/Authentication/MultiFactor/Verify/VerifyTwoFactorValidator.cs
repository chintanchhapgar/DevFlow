using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Verify;

internal sealed class VerifyTwoFactorValidator
    : AbstractValidator<VerifyTwoFactorCommand>
{
    public VerifyTwoFactorValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6)
            .Matches(@"^\d{6}$");
    }
}
