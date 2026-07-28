using FluentValidation;

namespace DevFlow.Project.Application.WorkItems.ChangeStatus;

internal sealed class ChangeWorkItemStatusCommandValidator
    : AbstractValidator<ChangeWorkItemStatusCommand>
{
    public ChangeWorkItemStatusCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
