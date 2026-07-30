using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.StartTimer;

public sealed record StartTimerCommand(
    Guid WorkItemId,
    string? Description)
    : IRequest<Result<StartTimerResponse>>;
