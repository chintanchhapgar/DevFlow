namespace DevFlow.Project.Application.Worklogs.Create;

public sealed record CreateWorklogRequest(
    Guid WorkItemId,
    string? Description,
    DateTime StartedAtUtc,
    DateTime EndedAtUtc);
