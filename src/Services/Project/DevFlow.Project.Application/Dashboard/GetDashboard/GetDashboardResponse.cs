namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record GetDashboardResponse(
    DashboardProjectResponse Project,
    DashboardMetricsResponse Metrics,
    DashboardSprintResponse? ActiveSprint,
    IReadOnlyList<DashboardAssignedWorkItemResponse> AssignedToMe,
    IReadOnlyList<DashboardRecentActivityResponse> RecentActivities);
