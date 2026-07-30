using DevFlow.BuildingBlocks.Infrastructure.Persistence.Pagination;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Pagination;
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

    public async Task<PagedList<WorkItemAggregate>> GetPagedAsync(
    Guid projectId,
    PaginationRequest pagination,
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
                EF.Functions.ILike(
                    x.Title,
                    $"%{search}%") ||

                EF.Functions.ILike(
                    x.Key,
                    $"%{search}%"));
        }

        if (status.HasValue)
        {
            query = query.Where(
                x => x.Status == status.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(
                x => x.Type == type.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(
                x => x.Priority == priority.Value);
        }

        if (assigneeId.HasValue)
        {
            query = query.Where(
                x => x.AssigneeId == assigneeId.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToPagedListAsync(
                pagination,
                cancellationToken);
    }

    public async Task<IReadOnlyList<WorkItemAggregate>> GetBySprintAsync(
        Guid sprintId,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .Where(x =>
                x.SprintId == sprintId &&
                !x.IsDeleted)
            .OrderBy(x => x.Status)
            .ThenBy(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkItemAggregate>> GetBacklogAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .Where(x =>
                x.ProjectId == projectId &&
                x.SprintId == null &&
                !x.IsDeleted)
            .OrderBy(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextSubtaskSequenceAsync(
    Guid parentId,
    CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .Where(x => x.ParentId == parentId)
            .CountAsync(cancellationToken) + 1;
    }
}
