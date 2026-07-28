using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.GetAll;

public sealed record GetAllWorkItemsResponse(
    IReadOnlyList<GetAllWorkItemItem> Items,
    int TotalCount,
    int Page,
    int PageSize);

public sealed record GetAllWorkItemItem(
    Guid Id,
    string Key,
    string Title,
    WorkItemType Type,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid? AssigneeId,
    DateTime? DueDate);
