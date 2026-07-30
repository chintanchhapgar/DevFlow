using DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;
using DevFlow.Project.Domain.Projects.Entities;

namespace DevFlow.Project.Infrastructure.Persistence.Sorting;

internal sealed class ProjectSorting
    : Sorting<ProjectAggregate>
{
    public ProjectSorting()
    {
        Map("name", x => x.Name);
        Map("key", x => x.Key);
        Map("createdOnUtc", x => x.CreatedOnUtc);
        Map("updatedOnUtc", x => x.UpdatedOnUtc);
        Map("status", x => x.Status);
        Map("visibility", x => x.Visibility);
    }
}
