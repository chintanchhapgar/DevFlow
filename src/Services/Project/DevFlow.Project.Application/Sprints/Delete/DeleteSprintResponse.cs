namespace DevFlow.Project.Application.Sprints.Delete;

public sealed record DeleteSprintResponse(
    Guid SprintId,
    string Status);
