using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.ChangePassword;

internal sealed class ChangePasswordValidator
    : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .NotEqual(x => x.CurrentPassword);
    }
}
