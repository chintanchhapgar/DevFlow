using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Backlog.RemoveFromSprint;

public sealed record RemoveWorkItemFromSprintCommand(
    Guid WorkItemId)
    : IRequest<Result<RemoveWorkItemFromSprintResponse>>;
