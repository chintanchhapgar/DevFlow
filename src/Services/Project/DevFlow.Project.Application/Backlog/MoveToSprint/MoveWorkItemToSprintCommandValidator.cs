using FluentValidation;

namespace DevFlow.Project.Application.Backlog.MoveToSprint;

internal sealed class MoveWorkItemToSprintCommandValidator
    : AbstractValidator<MoveWorkItemToSprintCommand>
{
    public MoveWorkItemToSprintCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.SprintId)
            .NotEmpty();
    }
}
