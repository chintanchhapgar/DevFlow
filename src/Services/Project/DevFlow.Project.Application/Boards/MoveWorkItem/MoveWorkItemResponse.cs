using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Boards.MoveWorkItem;

public sealed record MoveWorkItemResponse(
    Guid WorkItemId,
    WorkItemStatus Status);
