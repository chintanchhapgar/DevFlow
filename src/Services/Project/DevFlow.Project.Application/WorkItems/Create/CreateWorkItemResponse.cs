namespace DevFlow.Project.Application.WorkItems.Create;

public sealed record CreateWorkItemResponse(
    Guid WorkItemId,
    string Key,
    string Title);
