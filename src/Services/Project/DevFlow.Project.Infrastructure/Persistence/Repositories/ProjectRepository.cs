using DevFlow.BuildingBlocks.Infrastructure.Persistence.Pagination;
using DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Entities;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Project.Infrastructure.Persistence.Sorting;
using DevFlow.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class ProjectRepository
    : IProjectRepository
{
    private readonly ProjectDbContext _context;
    private readonly ProjectSorting _sorting;
    public ProjectRepository(
    ProjectDbContext context,
    ProjectSorting sorting)
    {
        _context = context;
        _sorting = sorting;

        Console.WriteLine($"Repository DbContext: {_context.ContextId}");
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

    public async Task<PagedList<ProjectAggregate>> GetPagedAsync(
    PaginationRequest pagination,
    string? search,
    Guid? memberId,
    CancellationToken cancellationToken = default)
    {
        IQueryable<ProjectAggregate> query =
            _context.Projects
                .Include(x => x.Members)
                .AsNoTracking();

        if (memberId.HasValue)
        {
            query = query.Where(x =>
                x.Members.Any(member => member.UserId == memberId.Value));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(x.Name, $"%{search}%") ||
                EF.Functions.ILike(x.Key, $"%{search}%"));
        }

        query = _sorting.Apply(
            query,
            pagination.SortBy,
            pagination.IsDescending);

        return await query.ToPagedListAsync(
            pagination,
            cancellationToken);
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
