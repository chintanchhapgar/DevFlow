using FluentValidation;

namespace DevFlow.Project.Application.Worklogs.StopTimer;

internal sealed class StopTimerCommandValidator
    : AbstractValidator<StopTimerCommand>
{
    public StopTimerCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();
    }
}
