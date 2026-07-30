using FluentValidation;

namespace DevFlow.Project.Application.Epics.RemoveWorkItem;

internal sealed class RemoveWorkItemCommandValidator
    : AbstractValidator<RemoveWorkItemCommand>
{
    public RemoveWorkItemCommandValidator()
    {
        RuleFor(x => x.EpicId)
            .NotEmpty();

        RuleFor(x => x.WorkItemId)
            .NotEmpty();
    }
}
