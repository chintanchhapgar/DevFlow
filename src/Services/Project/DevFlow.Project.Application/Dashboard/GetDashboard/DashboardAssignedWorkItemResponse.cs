using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record DashboardAssignedWorkItemResponse(
    Guid WorkItemId,
    string Key,
    string Title,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    DateTime? DueDate);
