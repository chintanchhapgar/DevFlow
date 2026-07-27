using FluentValidation;

namespace DevFlow.Project.Application.Projects.GetById;

internal sealed class GetProjectByIdQueryValidator
    : AbstractValidator<GetProjectByIdQuery>
{
    public GetProjectByIdQueryValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();
    }
}
