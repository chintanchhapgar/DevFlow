using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Reports.Velocity;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Project.Domain.WorkItems.Enums;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class VelocityRepository : IVelocityRepository
{
    private readonly ProjectDbContext _context;

    public VelocityRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<GetVelocityResponse?> GetAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == new ProjectId(projectId),
                cancellationToken);

        if (project is null)
        {
            return null;
        }

        var sprints = await _context.Sprints
            .AsNoTracking()
            .Where(x =>
                x.ProjectId == projectId &&
                !x.IsDeleted)
            .OrderBy(x => x.StartDate)
            .ToListAsync(cancellationToken);

        var result = new List<VelocitySprintResponse>();

        foreach (var sprint in sprints)
        {
            var committed = await _context.WorkItems
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.SprintId == sprint.Id.Value &&
                        !x.IsDeleted,
                    cancellationToken);

            var completed = await _context.WorkItems
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.SprintId == sprint.Id.Value &&
                        !x.IsDeleted &&
                        x.Status == WorkItemStatus.Done,
                    cancellationToken);

            result.Add(
                new VelocitySprintResponse(
                    sprint.Id.Value,
                    sprint.Name,
                    committed,
                    completed));
        }

        return new GetVelocityResponse(
            project.Id.Value,
            result);
    }
}
