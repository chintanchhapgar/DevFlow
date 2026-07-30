using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Events;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.WorkItems.Entities;

public sealed class WorkItemAggregate
    : AggregateRoot<WorkItemId>
{
    private WorkItemAggregate(
        WorkItemId id,
        Guid projectId,
        string key,
        string title,
        string? description,
        WorkItemType type,
        WorkItemPriority priority,
        Guid reporterId,
        Guid? assigneeId,
        DateTime? dueDate,
        decimal? estimateHours)
        : base(id)
    {
        ProjectId = projectId;
        Key = key;
        Title = title;
        Description = description;

        Type = type;
        Priority = priority;

        ReporterId = reporterId;
        AssigneeId = assigneeId;

        DueDate = dueDate;
        EstimateHours = estimateHours;

        Status = WorkItemStatus.Todo;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemCreatedDomainEvent(Id));
    }

    private WorkItemAggregate()
        : base(WorkItemId.Empty())
    {
    }

    public Guid ProjectId { get; private set; }

    public string Key { get; private set; } = string.Empty;

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public WorkItemType Type { get; private set; }

    public WorkItemStatus Status { get; private set; }

    public WorkItemPriority Priority { get; private set; }

    public Guid? AssigneeId { get; private set; }

    public Guid ReporterId { get; private set; }

    public Guid? EpicId { get; private set; }

    public Guid? ParentId { get; private set; }
    public bool IsSubtask => ParentId.HasValue;
    public int ChildCount { get; private set; }
    public Guid? SprintId { get; private set; }

    public decimal? EstimateHours { get; private set; }

    public DateTime? DueDate { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    private readonly List<WorkItemLabel> _labels = [];
    public IReadOnlyCollection<WorkItemLabel> Labels
    => _labels.AsReadOnly();

    public static WorkItemAggregate Create(
        Guid projectId,
        string key,
        string title,
        string? description,
        WorkItemType type,
        WorkItemPriority priority,
        Guid reporterId,
        Guid? assigneeId,
        DateTime? dueDate,
        decimal? estimateHours)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        return new WorkItemAggregate(
            WorkItemId.New(),
            projectId,
            key.Trim().ToUpperInvariant(),
            title.Trim(),
            description?.Trim(),
            type,
            priority,
            reporterId,
            assigneeId,
            dueDate,
            estimateHours);
    }

    public void Update(
        string title,
        string? description,
        DateTime? dueDate,
        decimal? estimateHours)
    {
        Title = title.Trim();
        Description = description?.Trim();

        DueDate = dueDate;
        EstimateHours = estimateHours;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemUpdatedDomainEvent(Id));
    }

    public void Assign(Guid userId)
    {
        if (AssigneeId == userId)
            return;

        AssigneeId = userId;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemAssignedDomainEvent(
                Id,
                userId));
    }

    public void Unassign()
    {
        AssigneeId = null;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ChangeStatus(
        WorkItemStatus status)
    {
        if (Status == status)
            return;

        Status = status;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemStatusChangedDomainEvent(
                Id,
                status));
    }

    public void ChangePriority(
        WorkItemPriority priority)
    {
        if (Priority == priority)
            return;

        Priority = priority;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemPriorityChangedDomainEvent(
                Id,
                priority));
    }

    public void MoveToSprint(Guid sprintId)
    {
        if (SprintId == sprintId)
            return;

        SprintId = sprintId;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void LinkEpic(Guid epicId)
    {
        EpicId = epicId;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void SetParent(Guid parentId)
    {
        ParentId = parentId;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemDeletedDomainEvent(Id));
    }

    public void Restore()
    {
        IsDeleted = false;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MoveToStatus(
    WorkItemStatus status)
    {
        if (Status == status)
            return;

        Status = status;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorkItemStatusChangedDomainEvent(
                Id,
                status));
    }

    public void RemoveFromSprint()
    {
        if (SprintId is null)
            return;

        SprintId = null;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public int GetNextChildSequence()
    {
        if (IsSubtask)
        {
            throw new InvalidOperationException(
                "Subtasks cannot have child work items.");
        }

        ChildCount++;

        UpdatedOnUtc = DateTime.UtcNow;

        return ChildCount;
    }

    public void AddLabel(Guid labelId)
    {
        if (_labels.Any(x => x.LabelId == labelId))
            return;

        _labels.Add(
            WorkItemLabel.Create(
                Id.Value,
                labelId));

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void RemoveLabel(Guid labelId)
    {
        var label = _labels.FirstOrDefault(
            x => x.LabelId == labelId);

        if (label is null)
            return;

        _labels.Remove(label);

        UpdatedOnUtc = DateTime.UtcNow;
    }
}
