using FluentValidation;

namespace DevFlow.Project.Application.Projects.Invitations.Decline;

internal sealed class DeclineProjectInvitationValidator
    : AbstractValidator<DeclineProjectInvitationCommand>
{
    public DeclineProjectInvitationValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
