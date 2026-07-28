using FluentValidation;

namespace DevFlow.Project.Application.WorkItems.Create;

internal sealed class CreateWorkItemCommandValidator
    : AbstractValidator<CreateWorkItemCommand>
{
    public CreateWorkItemCommandValidator()
    {
        RuleFor(x => x.ProjectId)
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
