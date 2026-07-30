namespace DevFlow.Project.Application.Worklogs.Summary;

public sealed record GetWorklogSummaryResponse(
    Guid WorkItemId,
    int TotalMinutes,
    decimal TotalHours,
    int TotalEntries,
    bool HasRunningTimer);
