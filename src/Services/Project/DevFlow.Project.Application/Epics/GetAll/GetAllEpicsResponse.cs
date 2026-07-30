namespace DevFlow.Project.Application.Epics.GetAll;

public sealed record GetAllEpicsResponse(
    Guid EpicId,
    Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate);
