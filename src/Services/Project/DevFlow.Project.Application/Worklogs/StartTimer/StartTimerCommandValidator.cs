using FluentValidation;

namespace DevFlow.Project.Application.Worklogs.StartTimer;

internal sealed class StartTimerCommandValidator
    : AbstractValidator<StartTimerCommand>
{
    public StartTimerCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(4000);
    }
}
