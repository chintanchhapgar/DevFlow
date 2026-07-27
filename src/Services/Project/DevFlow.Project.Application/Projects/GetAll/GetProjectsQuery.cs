using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.GetAll;

public sealed record GetProjectsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null)
    : IRequest<Result<GetProjectsResponse>>;
