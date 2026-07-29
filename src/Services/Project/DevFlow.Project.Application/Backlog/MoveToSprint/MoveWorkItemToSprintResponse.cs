namespace DevFlow.Project.Application.Backlog.MoveToSprint;

public sealed record MoveWorkItemToSprintResponse(
    Guid WorkItemId,
    Guid SprintId);
