using FluentValidation;

namespace DevFlow.Project.Application.WorkItems.Assign;

internal sealed class AssignWorkItemCommandValidator
    : AbstractValidator<AssignWorkItemCommand>
{
    public AssignWorkItemCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.AssigneeId)
            .NotEmpty();
    }
}
