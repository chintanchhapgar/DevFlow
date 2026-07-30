using FluentValidation;

namespace DevFlow.Project.Application.Labels.Create;

internal sealed class CreateLabelCommandValidator
    : AbstractValidator<CreateLabelCommand>
{
    public CreateLabelCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(20);
    }
}
