using DevFlow.Project.Domain.Epics.Entities;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.Project.Domain.Epics.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class EpicRepository
    : IEpicRepository
{
    private readonly ProjectDbContext _context;

    public EpicRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        EpicAggregate epic,
        CancellationToken cancellationToken = default)
    {
        await _context.Epics.AddAsync(
            epic,
            cancellationToken);
    }

    public async Task<EpicAggregate?> GetByIdAsync(
        EpicId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Epics
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<EpicAggregate?> GetByNameAsync(
        Guid projectId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Epics
            .FirstOrDefaultAsync(
                x => x.ProjectId == projectId &&
                     x.Name == name,
                cancellationToken);
    }

    public async Task<IReadOnlyList<EpicAggregate>> GetByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Epics
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}
