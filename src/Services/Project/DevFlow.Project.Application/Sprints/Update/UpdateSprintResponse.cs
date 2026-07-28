namespace DevFlow.Project.Application.Sprints.Update;

public sealed record UpdateSprintResponse(
    Guid SprintId,
    string Name);
