using FluentValidation;

namespace DevFlow.Project.Application.Projects.Invitations.Accept;

internal sealed class AcceptProjectInvitationValidator
    : AbstractValidator<AcceptProjectInvitationCommand>
{
    public AcceptProjectInvitationValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
