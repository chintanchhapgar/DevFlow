namespace DevFlow.Project.Application.Worklogs.StartTimer;

public sealed record StartTimerRequest(
    Guid WorkItemId,
    string? Description);
