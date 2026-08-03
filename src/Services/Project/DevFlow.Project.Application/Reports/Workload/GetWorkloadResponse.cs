namespace DevFlow.Project.Application.Reports.Workload;

public sealed record GetWorkloadResponse(
    Guid ProjectId,
    IReadOnlyList<WorkloadMemberResponse> Members);
