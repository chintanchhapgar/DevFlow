namespace DevFlow.Project.Application.Epics.RemoveWorkItem;

public sealed record RemoveWorkItemResponse(
    Guid EpicId,
    Guid WorkItemId);
