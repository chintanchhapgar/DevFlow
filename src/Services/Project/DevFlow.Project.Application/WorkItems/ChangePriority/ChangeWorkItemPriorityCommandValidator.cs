using FluentValidation;

namespace DevFlow.Project.Application.WorkItems.ChangePriority;

internal sealed class ChangeWorkItemPriorityCommandValidator
    : AbstractValidator<ChangeWorkItemPriorityCommand>
{
    public ChangeWorkItemPriorityCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}
