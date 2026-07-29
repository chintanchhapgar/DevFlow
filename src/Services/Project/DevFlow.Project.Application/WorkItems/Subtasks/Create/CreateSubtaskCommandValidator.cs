using FluentValidation;

namespace DevFlow.Project.Application.WorkItems.Subtasks.Create;

internal sealed class CreateSubtaskCommandValidator
    : AbstractValidator<CreateSubtaskCommand>
{
    public CreateSubtaskCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
