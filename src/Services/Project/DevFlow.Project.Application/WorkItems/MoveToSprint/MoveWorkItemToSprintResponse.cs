namespace DevFlow.Project.Application.WorkItems.MoveToSprint;

public sealed record MoveWorkItemToSprintResponse(
    Guid WorkItemId,
    Guid SprintId);
