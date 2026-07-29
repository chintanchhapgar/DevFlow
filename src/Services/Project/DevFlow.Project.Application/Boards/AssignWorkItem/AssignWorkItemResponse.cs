namespace DevFlow.Project.Application.Boards.AssignWorkItem;

public sealed record AssignWorkItemResponse(
    Guid WorkItemId,
    Guid AssigneeId);
