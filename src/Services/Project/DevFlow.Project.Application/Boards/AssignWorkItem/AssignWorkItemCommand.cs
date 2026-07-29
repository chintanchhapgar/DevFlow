using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.AssignWorkItem;

public sealed record AssignWorkItemCommand(
    Guid WorkItemId,
    Guid AssigneeId)
    : IRequest<Result<AssignWorkItemResponse>>;
