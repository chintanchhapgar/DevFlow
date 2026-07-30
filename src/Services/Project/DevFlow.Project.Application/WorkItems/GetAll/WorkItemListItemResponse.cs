using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.GetAll;

public sealed record WorkItemListItemResponse(
    Guid Id,
    string Key,
    string Title,
    WorkItemType Type,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid? AssigneeId,
    DateTime? DueDate);
