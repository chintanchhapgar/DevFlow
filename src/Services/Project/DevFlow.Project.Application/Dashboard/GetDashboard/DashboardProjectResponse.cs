namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed record DashboardProjectResponse(
    Guid ProjectId,
    string Key,
    string Name,
    string? Description,
    int MemberCount);
