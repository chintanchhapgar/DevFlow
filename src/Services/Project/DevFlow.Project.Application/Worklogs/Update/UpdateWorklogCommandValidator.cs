using FluentValidation;

namespace DevFlow.Project.Application.Worklogs.Update;

internal sealed class UpdateWorklogCommandValidator
    : AbstractValidator<UpdateWorklogCommand>
{
    public UpdateWorklogCommandValidator()
    {
        RuleFor(x => x.WorklogId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(4000);

        RuleFor(x => x.EndedAtUtc)
            .GreaterThan(x => x.StartedAtUtc);
    }
}
