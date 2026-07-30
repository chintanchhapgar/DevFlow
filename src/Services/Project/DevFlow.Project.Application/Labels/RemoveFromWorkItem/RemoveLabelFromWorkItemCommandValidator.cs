using FluentValidation;

namespace DevFlow.Project.Application.Labels.RemoveFromWorkItem;

internal sealed class RemoveLabelFromWorkItemCommandValidator
    : AbstractValidator<RemoveLabelFromWorkItemCommand>
{
    public RemoveLabelFromWorkItemCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.LabelId)
            .NotEmpty();
    }
}
