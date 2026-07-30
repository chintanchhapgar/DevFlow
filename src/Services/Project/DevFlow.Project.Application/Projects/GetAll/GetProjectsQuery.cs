using DevFlow.SharedKernel.Pagination;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetAll;

public sealed record GetProjectsQuery(
    PaginationRequest Pagination,
    string? Search)
    : IRequest<Result<PagedList<ProjectListItemResponse>>>;
