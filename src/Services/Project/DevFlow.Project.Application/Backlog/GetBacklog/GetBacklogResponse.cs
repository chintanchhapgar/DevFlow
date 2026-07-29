using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Backlog.GetBacklog;

public sealed record BacklogWorkItemResponse(
    Guid WorkItemId,
    string Key,
    string Title,
    WorkItemType Type,
    WorkItemPriority Priority,
    WorkItemStatus Status,
    Guid? AssigneeId,
    decimal? EstimateHours);

public sealed record GetBacklogResponse(
    IReadOnlyList<BacklogWorkItemResponse> Items);
