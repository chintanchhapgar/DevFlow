using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.ChangeStatus;

public sealed record ChangeWorkItemStatusRequest(
    WorkItemStatus Status);
