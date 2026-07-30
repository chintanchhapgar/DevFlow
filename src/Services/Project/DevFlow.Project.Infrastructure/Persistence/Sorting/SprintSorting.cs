using DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;
using DevFlow.Project.Domain.Sprints.Entities;

namespace DevFlow.Project.Infrastructure.Persistence.Sorting;

internal sealed class SprintSorting
    : Sorting<SprintAggregate>
{
    public SprintSorting()
    {
        Map("name", x => x.Name);
        Map("goal", x => x.Goal!);
        Map("status", x => x.Status);
        Map("startDate", x => x.StartDate);
        Map("endDate", x => x.EndDate);
        Map("createdOnUtc", x => x.CreatedOnUtc);
        Map("updatedOnUtc", x => x.UpdatedOnUtc);
    }
}
