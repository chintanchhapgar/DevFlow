using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.ChangePriority;

public sealed record ChangeWorkItemPriorityResponse(
    Guid WorkItemId,
    WorkItemPriority Priority);
