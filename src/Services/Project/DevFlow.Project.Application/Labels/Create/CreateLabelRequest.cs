namespace DevFlow.Project.Application.Labels.Create;

public sealed record CreateLabelRequest(
    Guid ProjectId,
    string Name,
    string Color);
