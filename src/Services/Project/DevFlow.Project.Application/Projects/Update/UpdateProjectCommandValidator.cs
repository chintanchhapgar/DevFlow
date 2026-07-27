using FluentValidation;

namespace DevFlow.Project.Application.Projects.Update;

internal sealed class UpdateProjectCommandValidator
    : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(2000);
    }
}
