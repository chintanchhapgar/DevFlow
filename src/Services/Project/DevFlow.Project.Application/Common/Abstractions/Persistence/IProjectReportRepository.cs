using DevFlow.Project.Application.Reports.ProjectSummary;

namespace DevFlow.Project.Application.Common.Abstractions.Persistence;

public interface IProjectReportRepository
{
    Task<GetProjectSummaryResponse?> GetProjectSummaryAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
}
