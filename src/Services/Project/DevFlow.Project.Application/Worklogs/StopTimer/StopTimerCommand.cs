using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.StopTimer;

public sealed record StopTimerCommand(
    Guid WorkItemId)
    : IRequest<Result<StopTimerResponse>>;
