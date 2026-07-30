namespace DevFlow.Project.Application.Epics.Create;

public sealed record CreateEpicResponse(
    Guid EpicId,
    Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate);
