namespace DevFlow.Project.Application.Epics.Delete;

public sealed record DeleteEpicResponse(
    Guid EpicId,
    Guid ProjectId,
    string Name,
    DateTime DeletedOnUtc);
