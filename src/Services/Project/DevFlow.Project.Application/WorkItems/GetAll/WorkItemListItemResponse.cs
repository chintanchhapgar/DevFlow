using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.GetAll;

public sealed record WorkItemListItemResponse(
    Guid Id,
    string Key,
    string Title,
    string? Description,
    WorkItemType Type,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid? AssigneeId,
    Guid? SprintId,
    decimal? EstimateHours,
    DateTime? DueDate);
