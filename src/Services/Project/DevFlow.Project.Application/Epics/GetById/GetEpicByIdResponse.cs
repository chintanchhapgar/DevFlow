namespace DevFlow.Project.Application.Epics.GetById;

public sealed record GetEpicByIdResponse(
    Guid EpicId,
    Guid ProjectId,
    string Name,
    string? Description,
    string Color,
    DateTime? StartDate,
    DateTime? DueDate,
    DateTime CreatedOnUtc,
    DateTime? UpdatedOnUtc);
