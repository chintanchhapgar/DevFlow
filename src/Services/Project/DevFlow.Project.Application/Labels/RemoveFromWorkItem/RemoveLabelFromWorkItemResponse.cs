namespace DevFlow.Project.Application.Labels.RemoveFromWorkItem;

public sealed record RemoveLabelFromWorkItemResponse(
    Guid WorkItemId,
    Guid LabelId);
