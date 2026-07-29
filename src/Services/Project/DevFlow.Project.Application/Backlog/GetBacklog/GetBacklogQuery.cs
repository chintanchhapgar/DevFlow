using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Backlog.GetBacklog;

public sealed record GetBacklogQuery(
    Guid ProjectId)
    : IRequest<Result<GetBacklogResponse>>;
