using DevFlow.BuildingBlocks.Infrastructure.Persistence.Pagination;
using DevFlow.Project.Domain.Sprints.Entities;
using DevFlow.Project.Domain.Sprints.Enums;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class SprintRepository
    : ISprintRepository
{
    private readonly ProjectDbContext _context;

    public SprintRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        SprintAggregate sprint,
        CancellationToken cancellationToken)
    {
        await _context.Sprints.AddAsync(
            sprint,
            cancellationToken);
    }

    public async Task<SprintAggregate?> GetByIdAsync(
        SprintId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Sprints
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<bool> ExistsAsync(
        SprintId id,
        CancellationToken cancellationToken = default)
    {
        return _context.Sprints.AnyAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public Task UpdateAsync(
        SprintAggregate sprint,
        CancellationToken cancellationToken)
    {
        _context.Sprints.Update(sprint);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        SprintAggregate sprint,
        CancellationToken cancellationToken)
    {
        _context.Sprints.Remove(sprint);

        return Task.CompletedTask;
    }

    public async Task<PagedList<SprintAggregate>> GetPagedAsync(
     Guid projectId,
     PaginationRequest pagination,
     string? search,
     CancellationToken cancellationToken = default)
    {
        IQueryable<SprintAggregate> query =
            _context.Sprints
                .Where(x =>
                    x.ProjectId == projectId &&
                    !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(
                    x.Name,
                    $"%{search}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToPagedListAsync(
                pagination,
                cancellationToken);
    }

    public async Task<SprintAggregate?> GetActiveSprintAsync(
    Guid projectId,
    CancellationToken cancellationToken = default)
    {
        return await _context.Sprints
            .FirstOrDefaultAsync(
                x =>
                    x.ProjectId == projectId &&
                    x.Status == SprintStatus.Active &&
                    !x.IsDeleted,
                cancellationToken);
    }
}
