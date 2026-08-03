namespace DevFlow.Project.Application.Reports.Workload;

public sealed record WorkloadWorkItemResponse(
    Guid Id,
    string Key,
    string Title,
    decimal? EstimateHours);
