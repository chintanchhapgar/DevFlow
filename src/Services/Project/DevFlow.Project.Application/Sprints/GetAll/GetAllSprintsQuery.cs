using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Pagination;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.GetAll;

public sealed record GetAllSprintsQuery(
    Guid ProjectId,
    PaginationRequest Pagination,
    string? Search = null)
    : IRequest<Result<PagedList<SprintListItemResponse>>>;
