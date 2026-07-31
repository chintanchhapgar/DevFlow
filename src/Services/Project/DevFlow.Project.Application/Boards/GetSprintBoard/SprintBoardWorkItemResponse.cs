using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Boards.GetSprintBoard;

public sealed record SprintBoardWorkItemResponse(
    Guid WorkItemId,
    string Key,
    string Title,
    WorkItemType Type,
    WorkItemPriority Priority,
    WorkItemStatus Status,
    Guid? AssigneeId,
    int ChildCount);
