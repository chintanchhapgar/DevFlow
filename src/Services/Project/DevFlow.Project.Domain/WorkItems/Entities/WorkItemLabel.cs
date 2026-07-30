namespace DevFlow.Project.Domain.WorkItems.Entities;

public sealed class WorkItemLabel
{
    private WorkItemLabel()
    {
    }

    private WorkItemLabel(
        Guid workItemId,
        Guid labelId)
    {
        WorkItemId = workItemId;
        LabelId = labelId;
    }

    public Guid WorkItemId { get; private set; }

    public Guid LabelId { get; private set; }

    public static WorkItemLabel Create(
        Guid workItemId,
        Guid labelId)
    {
        return new WorkItemLabel(
            workItemId,
            labelId);
    }
}
