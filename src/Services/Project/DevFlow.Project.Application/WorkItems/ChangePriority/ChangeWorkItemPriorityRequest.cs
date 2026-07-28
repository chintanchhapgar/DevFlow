using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.ChangePriority;

public sealed record ChangeWorkItemPriorityRequest(
    WorkItemPriority Priority);
