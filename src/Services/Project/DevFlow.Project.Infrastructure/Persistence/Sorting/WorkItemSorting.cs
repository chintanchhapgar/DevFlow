using DevFlow.BuildingBlocks.Infrastructure.Persistence.Sorting;
using DevFlow.Project.Domain.WorkItems.Entities;

namespace DevFlow.Project.Infrastructure.Persistence.Sorting;

internal sealed class WorkItemSorting
    : Sorting<WorkItemAggregate>
{
    public WorkItemSorting()
    {
        Map("key", x => x.Key);
        Map("title", x => x.Title);
        Map("type", x => x.Type);
        Map("status", x => x.Status);
        Map("priority", x => x.Priority);
        Map("assigneeId", x => x.AssigneeId);
        Map("dueDate", x => x.DueDate);
        Map("createdOnUtc", x => x.CreatedOnUtc);
        Map("updatedOnUtc", x => x.UpdatedOnUtc);
    }
}
