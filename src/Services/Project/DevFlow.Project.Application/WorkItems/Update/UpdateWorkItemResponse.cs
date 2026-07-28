namespace DevFlow.Project.Application.WorkItems.Update;

public sealed record UpdateWorkItemResponse(
    Guid WorkItemId,
    string Title);
