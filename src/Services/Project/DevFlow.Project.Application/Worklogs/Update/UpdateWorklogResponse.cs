namespace DevFlow.Project.Application.Worklogs.Update;

public sealed record UpdateWorklogResponse(
    Guid WorklogId,
    Guid WorkItemId,
    Guid UserId,
    string? Description,
    DateTime StartedAtUtc,
    DateTime EndedAtUtc,
    int MinutesSpent);
