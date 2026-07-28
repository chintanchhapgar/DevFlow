using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Assign;

public sealed record AssignWorkItemCommand(
    Guid WorkItemId,
    Guid AssigneeId)
    : IRequest<Result<AssignWorkItemResponse>>;
