
using DevFlow.Project.Domain.Worklogs.Events;
using DevFlow.Project.Domain.Worklogs.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Worklogs.Entities;

public sealed class WorklogAggregate : AggregateRoot<WorklogId>
{
    private WorklogAggregate()
    {
    }

    private WorklogAggregate(
        WorklogId id,
        Guid workItemId,
        Guid userId,
        string? description,
        DateTime startedAtUtc)
        : base(id)
    {
        WorkItemId = workItemId;
        UserId = userId;
        Description = description;

        StartedAtUtc = startedAtUtc;

        IsRunning = true;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorklogCreatedDomainEvent(Id));
    }

    public Guid WorkItemId { get; private set; }

    public Guid UserId { get; private set; }

    public string? Description { get; private set; }

    public DateTime StartedAtUtc { get; private set; }

    public DateTime? EndedAtUtc { get; private set; }

    public int MinutesSpent { get; private set; }

    public bool IsRunning { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public static WorklogAggregate Create(
    Guid workItemId,
    Guid userId,
    string? description,
    DateTime startedAtUtc,
    DateTime endedAtUtc)
    {
        var worklog = new WorklogAggregate(
            WorklogId.New(),
            workItemId,
            userId,
            description,
            startedAtUtc);

        worklog.EndedAtUtc = endedAtUtc;

        worklog.MinutesSpent =
            (int)(endedAtUtc - startedAtUtc).TotalMinutes;

        worklog.IsRunning = false;

        return worklog;
    }

    public static WorklogAggregate Start(
    Guid workItemId,
    Guid userId,
    string? description)
    {
        return new WorklogAggregate(
            WorklogId.New(),
            workItemId,
            userId,
            description,
            DateTime.UtcNow);
    }

    public void Stop(
        DateTime endedAtUtc)
    {
        if (!IsRunning)
            return;

        EndedAtUtc = endedAtUtc;

        MinutesSpent =
            (int)(EndedAtUtc.Value - StartedAtUtc).TotalMinutes;

        IsRunning = false;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorklogUpdatedDomainEvent(Id));
    }

    public void Update(
        string? description,
        DateTime startedAtUtc,
        DateTime endedAtUtc)
    {
        Description = description;

        StartedAtUtc = startedAtUtc;

        EndedAtUtc = endedAtUtc;

        MinutesSpent =
            (int)(endedAtUtc - startedAtUtc).TotalMinutes;

        IsRunning = false;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorklogUpdatedDomainEvent(Id));
    }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new WorklogDeletedDomainEvent(Id));
    }
}
