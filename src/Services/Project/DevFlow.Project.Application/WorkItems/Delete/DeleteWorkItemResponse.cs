namespace DevFlow.Project.Application.WorkItems.Delete;

public sealed record DeleteWorkItemResponse(
    Guid WorkItemId,
    string Status);
