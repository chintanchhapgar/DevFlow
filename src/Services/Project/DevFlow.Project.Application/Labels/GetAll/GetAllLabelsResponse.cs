namespace DevFlow.Project.Application.Labels.GetAll;

public sealed record GetAllLabelsResponse(
    Guid LabelId,
    Guid ProjectId,
    string Name,
    string Color);
