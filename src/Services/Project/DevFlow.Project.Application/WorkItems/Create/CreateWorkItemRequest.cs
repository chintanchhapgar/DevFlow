using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.Create;

public sealed record CreateWorkItemRequest(
    string Title,
    string? Description,
    WorkItemType Type,
    WorkItemPriority Priority,
    Guid? AssigneeId,
    DateTime? DueDate,
    decimal? EstimateHours);
