namespace DevFlow.Project.Application.Epics.AssignWorkItem;

public sealed record AssignWorkItemResponse(
    Guid EpicId,
    Guid WorkItemId);
