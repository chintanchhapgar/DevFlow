namespace DevFlow.Project.Application.Worklogs.StopTimer;

public sealed record StopTimerResponse(
    Guid WorklogId,
    Guid WorkItemId,
    DateTime StartedAtUtc,
    DateTime EndedAtUtc,
    int MinutesSpent);
