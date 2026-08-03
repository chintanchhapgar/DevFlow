using DevFlow.Project.Domain.WorkItems.Enums;

namespace DevFlow.Project.Application.Reports.ProjectSummary;

public sealed record GetProjectSummaryResponse(
    Guid ProjectId,
    string ProjectKey,
    string ProjectName,
    int TotalWorkItems,
    int TodoCount,
    int InProgressCount,
    int ReviewCount,
    int DoneCount,
    int TotalSprints,
    int ActiveSprints,
    int CompletedSprints,
    int TotalMembers);
