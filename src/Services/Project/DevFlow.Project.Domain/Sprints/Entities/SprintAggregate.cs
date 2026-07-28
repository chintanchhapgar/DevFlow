using DevFlow.Project.Domain.Sprints.Enums;
using DevFlow.Project.Domain.Sprints.Events;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Sprints.Entities;

public sealed class SprintAggregate
    : AggregateRoot<SprintId>
{
    private SprintAggregate(
        SprintId id,
        Guid projectId,
        string name,
        string? goal,
        DateOnly startDate,
        DateOnly endDate)
        : base(id)
    {
        ProjectId = projectId;

        Name = name.Trim();
        Goal = goal?.Trim();

        StartDate = startDate;
        EndDate = endDate;

        Status = SprintStatus.Planned;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new SprintCreatedDomainEvent(Id));
    }

    private SprintAggregate()
        : base(SprintId.Empty())
    {
    }

    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Goal { get; private set; }

    public SprintStatus Status { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public DateTime? StartedOnUtc { get; private set; }

    public DateTime? CompletedOnUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public static SprintAggregate Create(
        Guid projectId,
        string name,
        string? goal,
        DateOnly startDate,
        DateOnly endDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (endDate < startDate)
        {
            throw new ArgumentException(
                "End date must be after start date.");
        }

        return new SprintAggregate(
            SprintId.New(),
            projectId,
            name,
            goal,
            startDate,
            endDate);
    }

    public void Update(
        string name,
        string? goal,
        DateOnly startDate,
        DateOnly endDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (Status != SprintStatus.Planned)
        {
            throw new InvalidOperationException(
                "Only planned sprints can be edited.");
        }

        Name = name.Trim();
        Goal = goal?.Trim();
        StartDate = startDate;
        EndDate = endDate;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Start()
    {
        if (Status != SprintStatus.Planned)
        {
            throw new InvalidOperationException(
                "Sprint cannot be started.");
        }

        Status = SprintStatus.Active;
        StartedOnUtc = DateTime.UtcNow;
        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new SprintStartedDomainEvent(Id));
    }

    public void Complete()
    {
        if (Status != SprintStatus.Active)
        {
            throw new InvalidOperationException(
                "Only active sprints can be completed.");
        }

        Status = SprintStatus.Completed;
        CompletedOnUtc = DateTime.UtcNow;
        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new SprintCompletedDomainEvent(Id));
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}
