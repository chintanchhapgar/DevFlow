namespace DevFlow.Project.Application.Labels.Delete;

public sealed record DeleteLabelResponse(
    Guid LabelId,
    Guid ProjectId,
    string Name,
    DateTime DeletedOnUtc);
