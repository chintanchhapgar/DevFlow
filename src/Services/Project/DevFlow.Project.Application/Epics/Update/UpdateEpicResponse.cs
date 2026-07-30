namespace DevFlow.Project.Application.Epics.Update;

public sealed record UpdateEpicResponse(
    Guid EpicId,
    Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate);
