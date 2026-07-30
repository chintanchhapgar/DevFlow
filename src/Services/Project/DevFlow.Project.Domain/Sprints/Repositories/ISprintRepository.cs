
using DevFlow.Project.Domain.Sprints.Entities;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Pagination;

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

    Task<PagedList<SprintAggregate>> GetPagedAsync(
        Guid projectId,
        PaginationRequest pagination,
        string? search,
        CancellationToken cancellationToken = default);

    Task<SprintAggregate?> GetActiveSprintAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
}
