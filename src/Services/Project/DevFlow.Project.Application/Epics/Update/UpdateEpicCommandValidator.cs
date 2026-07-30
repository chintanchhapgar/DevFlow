using FluentValidation;

namespace DevFlow.Project.Application.Epics.Update;

internal sealed class UpdateEpicCommandValidator
    : AbstractValidator<UpdateEpicCommand>
{
    public UpdateEpicCommandValidator()
    {
        RuleFor(x => x.EpicId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(4000);

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(20);
    }
}
