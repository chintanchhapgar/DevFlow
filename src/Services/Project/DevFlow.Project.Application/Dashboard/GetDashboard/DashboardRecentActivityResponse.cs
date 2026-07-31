namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record DashboardRecentActivityResponse(
    Guid Id,
    string Type,
    string Message,
    DateTime CreatedOnUtc);
