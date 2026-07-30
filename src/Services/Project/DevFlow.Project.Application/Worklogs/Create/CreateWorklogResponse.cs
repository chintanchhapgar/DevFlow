namespace DevFlow.Project.Application.Worklogs.Create;

public sealed record CreateWorklogResponse(
    Guid WorklogId,
    Guid WorkItemId,
    Guid UserId,
    string? Description,
    DateTime StartedAtUtc,
    bool IsRunning);
