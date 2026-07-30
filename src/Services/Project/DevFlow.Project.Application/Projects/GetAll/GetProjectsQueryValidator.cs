using DevFlow.SharedKernel.Pagination;
using FluentValidation;

namespace DevFlow.Project.Application.Projects.GetAll;

internal sealed class GetProjectsQueryValidator
    : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator()
    {
        RuleFor(x => x.Pagination)
            .SetValidator(new PaginationRequestValidator());
    }
}
