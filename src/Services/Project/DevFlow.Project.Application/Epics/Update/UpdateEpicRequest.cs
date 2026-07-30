namespace DevFlow.Project.Application.Epics.Update;

public sealed record UpdateEpicRequest(
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate);
