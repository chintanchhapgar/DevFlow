namespace DevFlow.Project.Application.Worklogs.Update;

public sealed record UpdateWorklogRequest(
    string? Description,
    DateTime StartedAtUtc,
    DateTime EndedAtUtc);
