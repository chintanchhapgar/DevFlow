using FluentValidation;

namespace DevFlow.Project.Application.Projects.Invitations.Invite;

internal sealed class InviteProjectMemberValidator
    : AbstractValidator<InviteProjectMemberCommand>
{
    public InviteProjectMemberValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}
