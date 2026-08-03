namespace DevFlow.Project.Application.Reports.Workload;

public sealed record WorkloadMemberResponse(
    Guid UserId,
    int TotalWorkItems,
    decimal TotalEstimateHours,
    IReadOnlyList<WorkloadWorkItemResponse> WorkItems);
