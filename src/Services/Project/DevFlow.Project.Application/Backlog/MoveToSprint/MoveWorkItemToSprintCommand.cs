using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Backlog.MoveToSprint;

public sealed record MoveWorkItemToSprintCommand(
    Guid WorkItemId,
    Guid SprintId)
    : IRequest<Result<MoveWorkItemToSprintResponse>>;
