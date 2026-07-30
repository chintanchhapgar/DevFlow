using FluentValidation;

namespace DevFlow.Project.Application.Epics.Create;

internal sealed class CreateEpicCommandValidator
    : AbstractValidator<CreateEpicCommand>
{
    public CreateEpicCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Color)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Description)
            .MaximumLength(4000);
    }
}
