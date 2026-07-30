namespace DevFlow.Project.Application.Epics.Create;

public sealed record CreateEpicRequest(
    Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate);
