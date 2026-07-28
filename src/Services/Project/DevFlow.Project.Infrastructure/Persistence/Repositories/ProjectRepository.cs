using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Entities;
using DevFlow.Project.Domain.Projects.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository
    : IProjectRepository
{
    private readonly ProjectDbContext _context;

    public ProjectRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectAggregate?> GetByIdAsync(
        ProjectId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Include(x => x.Members)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<ProjectAggregate?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        return await _context.Projects
            .Include(x => x.Members)
            .FirstOrDefaultAsync(
                x => x.Key == key,
                cancellationToken);
    }

    public Task<bool> ExistsByKeyAsync(
        string key,
        CancellationToken cancellationToken)
    {
        return _context.Projects.AnyAsync(
            x => x.Key == key,
            cancellationToken);
    }

    public async Task AddAsync(
        ProjectAggregate project,
        CancellationToken cancellationToken)
    {
        await _context.Projects.AddAsync(
            project,
            cancellationToken);
    }

    public Task UpdateAsync(
        ProjectAggregate project,
        CancellationToken cancellationToken)
    {
        _context.Projects.Update(project);

        return Task.CompletedTask;
    }

    public async Task<(IReadOnlyList<ProjectAggregate> Projects, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ProjectAggregate> query =
            _context.Projects
                .Include(x => x.Members);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
            EF.Functions.ILike(x.Name, $"%{search}%") ||
            EF.Functions.ILike(x.Key, $"%{search}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var projects = await query
            .OrderBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (projects, totalCount);
    }

    public async Task<ProjectAggregate?> GetByInvitationTokenAsync(
        Guid token,
        CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .Include(x => x.Members)
            .Include(x => x.Invitations)
            .FirstOrDefaultAsync(
                x => x.Invitations.Any(i => i.Token == token),
                cancellationToken);
    }
}
