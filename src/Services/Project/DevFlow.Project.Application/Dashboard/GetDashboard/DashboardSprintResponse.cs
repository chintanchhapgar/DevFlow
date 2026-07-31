namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record DashboardSprintResponse(
    Guid SprintId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    int RemainingDays,
    double CompletionPercentage);
