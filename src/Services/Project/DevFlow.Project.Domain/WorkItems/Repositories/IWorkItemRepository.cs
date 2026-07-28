using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.ValueObjects;

namespace DevFlow.Project.Domain.WorkItems.Repositories;

public interface IWorkItemRepository
{
    Task AddAsync(
        WorkItemAggregate workItem,
        CancellationToken cancellationToken);

    Task<WorkItemAggregate?> GetByIdAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default);

    Task<WorkItemAggregate?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        WorkItemAggregate workItem,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        WorkItemAggregate workItem,
        CancellationToken cancellationToken);

    Task<(IReadOnlyList<WorkItemAggregate> Items, int TotalCount)> GetPagedAsync(
        Guid projectId,
        int page,
        int pageSize,
        string? search,
        WorkItemStatus? status,
        WorkItemType? type,
        WorkItemPriority? priority,
        Guid? assigneeId,
        CancellationToken cancellationToken = default);
}
