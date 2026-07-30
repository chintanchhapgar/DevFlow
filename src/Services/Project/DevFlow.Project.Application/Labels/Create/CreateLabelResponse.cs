namespace DevFlow.Project.Application.Labels.Create;

public sealed record CreateLabelResponse(
    Guid LabelId,
    Guid ProjectId,
    string Name,
    string Color);
