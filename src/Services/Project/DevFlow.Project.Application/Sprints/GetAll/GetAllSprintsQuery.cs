using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.GetAll;

public sealed record GetAllSprintsQuery(
    Guid ProjectId,
    int Page = 1,
    int PageSize = 20,
    string? Search = null)
    : IRequest<Result<GetAllSprintsResponse>>;
