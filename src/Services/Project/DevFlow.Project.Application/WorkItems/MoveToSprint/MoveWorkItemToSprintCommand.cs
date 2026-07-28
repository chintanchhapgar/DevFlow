using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.MoveToSprint;

public sealed record MoveWorkItemToSprintCommand(
    Guid WorkItemId,
    Guid SprintId)
    : IRequest<Result<MoveWorkItemToSprintResponse>>;
