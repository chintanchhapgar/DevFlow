namespace DevFlow.Project.Application.Projects.Update;

public sealed record UpdateProjectResponse(
    Guid ProjectId,
    string Key,
    string Name);
