using FluentValidation;

namespace DevFlow.Project.Application.Projects.Members.Add;

internal sealed class AddProjectMemberCommandValidator
    : AbstractValidator<AddProjectMemberCommand>
{
    public AddProjectMemberCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}
