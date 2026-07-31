namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record DashboardMetricsResponse(
    int TotalWorkItems,
    int Todo,
    int InProgress,
    int Review,
    int Done);
