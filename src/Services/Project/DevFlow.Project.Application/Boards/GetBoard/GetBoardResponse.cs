using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Boards.GetBoard;

public sealed record BoardWorkItemResponse(
    Guid WorkItemId,
    string Key,
    string Title,
    WorkItemType Type,
    WorkItemPriority Priority,
    Guid? AssigneeId);

public sealed record BoardColumnResponse(
    WorkItemStatus Status,
    IReadOnlyList<BoardWorkItemResponse> Items);

public sealed record ActiveSprintResponse(
    Guid SprintId,
    string Name);

public sealed record GetBoardResponse(
    ActiveSprintResponse? Sprint,
    IReadOnlyList<BoardColumnResponse> Columns);
