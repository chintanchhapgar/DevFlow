namespace DevFlow.Project.Application.Projects.Create;

public sealed record CreateProjectResponse(
    Guid ProjectId,
    string Key,
    string Name);
