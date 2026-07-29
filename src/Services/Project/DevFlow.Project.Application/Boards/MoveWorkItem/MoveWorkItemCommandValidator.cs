using FluentValidation;

namespace DevFlow.Project.Application.Boards.MoveWorkItem;

internal sealed class MoveWorkItemCommandValidator
    : AbstractValidator<MoveWorkItemCommand>
{
    public MoveWorkItemCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();
    }
}
