using FluentValidation;

namespace DevFlow.Project.Application.Labels.AssignToWorkItem;

internal sealed class AssignLabelToWorkItemCommandValidator
    : AbstractValidator<AssignLabelToWorkItemCommand>
{
    public AssignLabelToWorkItemCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.LabelId)
            .NotEmpty();
    }
}
