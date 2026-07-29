using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.MoveWorkItem;

public sealed record MoveWorkItemCommand(
    Guid WorkItemId,
    WorkItemStatus Status)
    : IRequest<Result<MoveWorkItemResponse>>;
