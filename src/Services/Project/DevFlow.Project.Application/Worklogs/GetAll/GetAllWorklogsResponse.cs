namespace DevFlow.Project.Application.Worklogs.GetAll;

public sealed record GetAllWorklogsResponse(
    Guid WorklogId,
    Guid WorkItemId,
    Guid UserId,
    string? Description,
    DateTime StartedAtUtc,
    DateTime? EndedAtUtc,
    int MinutesSpent,
    bool IsRunning);
