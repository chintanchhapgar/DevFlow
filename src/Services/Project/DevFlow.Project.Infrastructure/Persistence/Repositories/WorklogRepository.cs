using DevFlow.Project.Domain.Worklogs.Entities;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.Project.Domain.Worklogs.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class WorklogRepository
    : IWorklogRepository
{
    private readonly ProjectDbContext _context;

    public WorklogRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        WorklogAggregate worklog,
        CancellationToken cancellationToken = default)
    {
        await _context.Worklogs.AddAsync(
            worklog,
            cancellationToken);
    }

    public async Task<WorklogAggregate?> GetByIdAsync(
        WorklogId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<WorklogAggregate>> GetByWorkItemAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .Where(x => x.WorkItemId == workItemId)
            .OrderByDescending(x => x.StartedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorklogAggregate>> GetByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.StartedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorklogAggregate?> GetRunningWorklogAsync(
        Guid workItemId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Worklogs
            .FirstOrDefaultAsync(
                x =>
                    x.WorkItemId == workItemId &&
                    x.UserId == userId &&
                    x.IsRunning,
                cancellationToken);
    }
}
