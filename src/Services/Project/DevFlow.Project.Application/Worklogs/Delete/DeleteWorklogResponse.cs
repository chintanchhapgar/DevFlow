namespace DevFlow.Project.Application.Worklogs.Delete;

public sealed record DeleteWorklogResponse(
    Guid WorklogId,
    Guid WorkItemId,
    DateTime DeletedOnUtc);
