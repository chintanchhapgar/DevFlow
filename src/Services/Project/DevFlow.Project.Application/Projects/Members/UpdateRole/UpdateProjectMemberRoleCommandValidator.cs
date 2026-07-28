using FluentValidation;

namespace DevFlow.Project.Application.Projects.Members.UpdateRole;

internal sealed class UpdateProjectMemberRoleCommandValidator
    : AbstractValidator<UpdateProjectMemberRoleCommand>
{
    public UpdateProjectMemberRoleCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}
