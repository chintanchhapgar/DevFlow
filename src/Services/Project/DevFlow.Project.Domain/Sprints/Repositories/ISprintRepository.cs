
using DevFlow.Project.Domain.Sprints.Entities;
using DevFlow.Project.Domain.Sprints.ValueObjects;

namespace DevFlow.Project.Domain.Sprints.Repositories;

public interface ISprintRepository
{
    Task AddAsync(
        SprintAggregate sprint,
        CancellationToken cancellationToken);

    Task<SprintAggregate?> GetByIdAsync(
        SprintId id,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        SprintId id,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        SprintAggregate sprint,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        SprintAggregate sprint,
        CancellationToken cancellationToken);

    Task<(IReadOnlyList<SprintAggregate> Items, int TotalCount)> GetPagedAsync(
        Guid projectId,
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default);
}
