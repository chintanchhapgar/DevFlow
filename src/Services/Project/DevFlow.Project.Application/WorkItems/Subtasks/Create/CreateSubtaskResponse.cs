namespace DevFlow.Project.Application.WorkItems.Subtasks.Create;

public sealed record CreateSubtaskResponse(
    Guid WorkItemId,
    Guid ParentId,
    string Key);
