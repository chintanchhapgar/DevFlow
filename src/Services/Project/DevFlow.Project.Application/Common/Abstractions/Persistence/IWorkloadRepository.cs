using DevFlow.Project.Application.Reports.Workload;

namespace DevFlow.Project.Application.Common.Abstractions.Persistence;

public interface IWorkloadRepository
{
    Task<GetWorkloadResponse?> GetAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
}
