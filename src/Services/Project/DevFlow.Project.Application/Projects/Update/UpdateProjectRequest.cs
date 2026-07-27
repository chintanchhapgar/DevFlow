namespace DevFlow.Project.Application.Projects.Update;

using DevFlow.Project.Domain.Projects.Enums;

public sealed record UpdateProjectRequest(
    string Name,
    string? Description,
    ProjectVisibility Visibility);
