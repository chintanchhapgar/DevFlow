using FluentValidation;

namespace DevFlow.Project.Application.Sprints.Update;

internal sealed class UpdateSprintCommandValidator
    : AbstractValidator<UpdateSprintCommand>
{
    public UpdateSprintCommandValidator()
    {
        RuleFor(x => x.SprintId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Goal)
            .MaximumLength(1000);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate);
    }
}
