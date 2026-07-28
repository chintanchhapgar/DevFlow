using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class WorkItemRepository
    : IWorkItemRepository
{
    private readonly ProjectDbContext _context;

    public WorkItemRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<WorkItemAggregate?> GetByIdAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<WorkItemAggregate?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);
    }

    public Task<bool> ExistsAsync(
        WorkItemId id,
        CancellationToken cancellationToken = default)
    {
        return _context.WorkItems.AnyAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public async Task AddAsync(
        WorkItemAggregate workItem,
        CancellationToken cancellationToken = default)
    {
        await _context.WorkItems.AddAsync(
            workItem,
            cancellationToken);
    }

    public Task UpdateAsync(
        WorkItemAggregate workItem,
        CancellationToken cancellationToken = default)
    {
        _context.WorkItems.Update(workItem);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        WorkItemAggregate workItem,
        CancellationToken cancellationToken = default)
    {
        _context.WorkItems.Remove(workItem);

        return Task.CompletedTask;
    }

    public async Task<(IReadOnlyList<WorkItemAggregate> Items, int TotalCount)> GetPagedAsync(
        Guid projectId,
        int page,
        int pageSize,
        string? search,
        WorkItemStatus? status,
        WorkItemType? type,
        WorkItemPriority? priority,
        Guid? assigneeId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<WorkItemAggregate> query =
            _context.WorkItems
                .Where(x =>
                    x.ProjectId == projectId &&
                    !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(x.Title, $"%{search}%") ||
                EF.Functions.ILike(x.Key, $"%{search}%"));
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status);
        }

        if (type.HasValue)
        {
            query = query.Where(x => x.Type == type);
        }

        if (priority.HasValue)
        {
            query = query.Where(x => x.Priority == priority);
        }

        if (assigneeId.HasValue)
        {
            query = query.Where(x => x.AssigneeId == assigneeId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
