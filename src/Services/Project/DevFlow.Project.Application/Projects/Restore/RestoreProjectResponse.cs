namespace DevFlow.Project.Application.Projects.Restore;

public sealed record RestoreProjectResponse(
    Guid ProjectId,
    string Key,
    string Name,
    string Status);
