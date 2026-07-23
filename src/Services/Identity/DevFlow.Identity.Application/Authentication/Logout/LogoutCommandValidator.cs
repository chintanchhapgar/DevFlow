using FluentValidation;

namespace DevFlow.Identity.Application.Authentication.Logout;

internal sealed class LogoutCommandValidator
    : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
