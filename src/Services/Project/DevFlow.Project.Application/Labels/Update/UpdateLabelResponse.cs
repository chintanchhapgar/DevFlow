namespace DevFlow.Project.Application.Labels.Update;

public sealed record UpdateLabelResponse(
    Guid LabelId,
    Guid ProjectId,
    string Name,
    string Color);
