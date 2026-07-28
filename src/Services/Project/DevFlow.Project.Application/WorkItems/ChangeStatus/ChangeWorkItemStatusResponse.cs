using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.WorkItems.ChangeStatus;

public sealed record ChangeWorkItemStatusResponse(
    Guid WorkItemId,
    WorkItemStatus Status);
