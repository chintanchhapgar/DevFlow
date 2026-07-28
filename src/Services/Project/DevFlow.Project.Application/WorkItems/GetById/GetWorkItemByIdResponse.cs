using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.GetById;

public sealed record GetWorkItemByIdResponse(
    Guid Id,
    Guid ProjectId,
    string Key,
    string Title,
    string? Description,
    WorkItemType Type,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid? AssigneeId,
    Guid ReporterId,
    Guid? EpicId,
    Guid? ParentId,
    Guid? SprintId,
    decimal? EstimateHours,
    DateTime? DueDate,
    DateTime CreatedOnUtc,
    DateTime? UpdatedOnUtc);
