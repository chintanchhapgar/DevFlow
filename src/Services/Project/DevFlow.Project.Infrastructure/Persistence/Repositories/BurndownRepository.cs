using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Reports.Burndown;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class BurndownRepository : IBurndownRepository
{
    private readonly ProjectDbContext _context;

    public BurndownRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<GetBurndownResponse?> GetAsync(
        Guid sprintId,
        CancellationToken cancellationToken = default)
    {
        var sprint = await _context.Sprints
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == new SprintId(sprintId),
                cancellationToken);

        if (sprint is null)
        {
            return null;
        }

        var workItems = await _context.WorkItems
            .AsNoTracking()
            .Where(x =>
                x.SprintId == sprintId &&
                !x.IsDeleted)
            .ToListAsync(cancellationToken);

        var total = workItems.Count;

        var duration =
            sprint.EndDate.DayNumber -
            sprint.StartDate.DayNumber + 1;

        var points = new List<BurndownPointResponse>();

        for (var i = 0; i < duration; i++)
        {
            var day = sprint.StartDate.AddDays(i);

            var completed = workItems.Count(x =>
                x.Status == Domain.WorkItems.Enums.WorkItemStatus.Done &&
                x.UpdatedOnUtc.HasValue &&
                DateOnly.FromDateTime(
                    x.UpdatedOnUtc.Value) <= day);

            var remaining = total - completed;

            var ideal =
                Math.Max(
                    0,
                    total -
                    (int)Math.Round(
                        (double)(i + 1) / duration * total));

            points.Add(
                new BurndownPointResponse(
                    day,
                    remaining,
                    completed,
                    ideal));
        }

        return new GetBurndownResponse(
            sprint.Id.Value,
            sprint.Name,
            points);
    }
}
