namespace DevFlow.Project.Application.Worklogs.StartTimer;

public sealed record StartTimerResponse(
    Guid WorklogId,
    Guid WorkItemId,
    DateTime StartedAtUtc);
