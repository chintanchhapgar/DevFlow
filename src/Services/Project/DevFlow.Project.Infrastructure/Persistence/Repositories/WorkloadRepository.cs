using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Reports.Workload;
using DevFlow.Project.Domain.Projects.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class WorkloadRepository : IWorkloadRepository
{
    private readonly ProjectDbContext _context;

    public WorkloadRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<GetWorkloadResponse?> GetAsync(
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

        var workItems = await _context.WorkItems
            .AsNoTracking()
            .Where(x =>
                x.ProjectId == projectId &&
                !x.IsDeleted &&
                x.AssigneeId.HasValue)
            .OrderBy(x => x.Key)
            .ToListAsync(cancellationToken);

        var members = workItems
            .GroupBy(x => x.AssigneeId!.Value)
            .Select(g =>
                new WorkloadMemberResponse(
                    g.Key,
                    g.Count(),
                    g.Sum(x => x.EstimateHours ?? 0),
                    g.Select(x =>
                        new WorkloadWorkItemResponse(
                            x.Id.Value,
                            x.Key,
                            x.Title,
                            x.EstimateHours))
                    .ToList()))
            .OrderByDescending(x => x.TotalEstimateHours)
            .ThenByDescending(x => x.TotalWorkItems)
            .ToList();

        return new GetWorkloadResponse(
            project.Id.Value,
            members);
    }
}
