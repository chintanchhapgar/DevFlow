using FluentValidation;

namespace DevFlow.Project.Application.Epics.AssignWorkItem;

internal sealed class AssignWorkItemCommandValidator
    : AbstractValidator<AssignWorkItemCommand>
{
    public AssignWorkItemCommandValidator()
    {
        RuleFor(x => x.EpicId)
            .NotEmpty();

        RuleFor(x => x.WorkItemId)
            .NotEmpty();
    }
}
