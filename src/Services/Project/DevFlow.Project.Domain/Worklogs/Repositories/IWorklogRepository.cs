using DevFlow.Project.Domain.Worklogs.Entities;
using DevFlow.Project.Domain.Worklogs.ValueObjects;

namespace DevFlow.Project.Domain.Worklogs.Repositories;

public interface IWorklogRepository
{
    Task AddAsync(
        WorklogAggregate worklog,
        CancellationToken cancellationToken = default);

    Task<WorklogAggregate?> GetByIdAsync(
        WorklogId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorklogAggregate>> GetByWorkItemAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorklogAggregate>> GetByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<WorklogAggregate?> GetRunningWorklogAsync(
        Guid workItemId,
        Guid userId,
        CancellationToken cancellationToken = default);
}
