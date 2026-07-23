using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.ResetPassword;

internal sealed class ResetPasswordValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8);
    }
}
