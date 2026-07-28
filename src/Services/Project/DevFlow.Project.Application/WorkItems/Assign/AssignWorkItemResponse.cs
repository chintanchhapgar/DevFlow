namespace DevFlow.Project.Application.WorkItems.Assign;

public sealed record AssignWorkItemResponse(
    Guid WorkItemId,
    Guid AssigneeId);
