using FluentValidation;

namespace DevFlow.Project.Application.Worklogs.Create;

internal sealed class CreateWorklogCommandValidator
    : AbstractValidator<CreateWorklogCommand>
{
    public CreateWorklogCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(4000);

        RuleFor(x => x.StartedAtUtc)
            .NotEmpty();
    }
}
