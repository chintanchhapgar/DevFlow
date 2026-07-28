using FluentValidation;

namespace DevFlow.Project.Application.WorkItems.Update;

internal sealed class UpdateWorkItemCommandValidator
    : AbstractValidator<UpdateWorkItemCommand>
{
    public UpdateWorkItemCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Description)
            .MaximumLength(5000);

        RuleFor(x => x.EstimateHours)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EstimateHours.HasValue);
    }
}
