using DevFlow.Project.Domain.Labels.Entities;
using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.Project.Domain.Labels.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class LabelRepository
    : ILabelRepository
{
    private readonly ProjectDbContext _context;

    public LabelRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Label label,
        CancellationToken cancellationToken = default)
    {
        await _context.Labels.AddAsync(
            label,
            cancellationToken);
    }

    public async Task<Label?> GetByIdAsync(
        LabelId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Labels
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<Label?> GetByNameAsync(
        Guid projectId,
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Labels
            .FirstOrDefaultAsync(
                x => x.ProjectId == projectId &&
                     x.Name == name,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Label>> GetByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Labels
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}
