namespace DevFlow.Project.Application.Labels.AssignToWorkItem;

public sealed record AssignLabelToWorkItemResponse(
    Guid WorkItemId,
    Guid LabelId);
