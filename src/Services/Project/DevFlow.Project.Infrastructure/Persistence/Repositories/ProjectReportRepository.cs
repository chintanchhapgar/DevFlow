using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Reports.ProjectSummary;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Project.Domain.Sprints.Enums;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class ProjectReportRepository : IProjectReportRepository
{
    private readonly ProjectDbContext _context;

    public ProjectReportRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<GetProjectSummaryResponse?> GetProjectSummaryAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var project = await _context.Projects
            .Include(x => x.Members)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == new ProjectId(projectId),
                cancellationToken);

        if (project is null)
        {
            return null;
        }

        var workItems = _context.WorkItems
            .AsNoTracking()
            .Where(x =>
                x.ProjectId == projectId &&
                !x.IsDeleted);

        var sprints = _context.Sprints
            .AsNoTracking()
            .Where(x =>
                x.ProjectId == projectId &&
                !x.IsDeleted);

        return new GetProjectSummaryResponse(
            project.Id.Value,
            project.Key,
            project.Name,

            await workItems.CountAsync(cancellationToken),

            await workItems.CountAsync(
                x => x.Status == WorkItemStatus.Todo,
                cancellationToken),

            await workItems.CountAsync(
                x => x.Status == WorkItemStatus.InProgress,
                cancellationToken),

            await workItems.CountAsync(
                x => x.Status == WorkItemStatus.InReview,
                cancellationToken),

            await workItems.CountAsync(
                x => x.Status == WorkItemStatus.Done,
                cancellationToken),

            await sprints.CountAsync(cancellationToken),

            await sprints.CountAsync(
                x => x.Status == SprintStatus.Active,
                cancellationToken),

            await sprints.CountAsync(
                x => x.Status == SprintStatus.Completed,
                cancellationToken),

            project.Members.Count);
    }
}
