using FluentValidation;

namespace DevFlow.Project.Application.Labels.Update;

internal sealed class UpdateLabelCommandValidator
    : AbstractValidator<UpdateLabelCommand>
{
    public UpdateLabelCommandValidator()
    {
        RuleFor(x => x.LabelId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(20);
    }
}
