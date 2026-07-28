namespace DevFlow.Project.Application.Sprints.Create;

public sealed record CreateSprintResponse(
    Guid SprintId,
    Guid ProjectId,
    string Name);
