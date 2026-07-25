using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Login;

internal sealed class CompleteTwoFactorLoginValidator
    : AbstractValidator<CompleteTwoFactorLoginCommand>
{
    public CompleteTwoFactorLoginValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        When(x => !x.IsRecoveryCode, () =>
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(6)
                .Matches(@"^\d{6}$")
                .WithMessage("Verification code must be a 6-digit number.");
        });

        When(x => x.IsRecoveryCode, () =>
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(32);
        });
    }
}
