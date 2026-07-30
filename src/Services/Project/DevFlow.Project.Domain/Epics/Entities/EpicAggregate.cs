using DevFlow.Project.Domain.Epics.Events;
using DevFlow.Project.Domain.Epics.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Epics.Entities;

public sealed class EpicAggregate
    : AggregateRoot<EpicId>
{
    private EpicAggregate(
        EpicId id,
        Guid projectId,
        string name,
        string? description,
        string color,
        DateTime? startDate,
        DateTime? dueDate)
        : base(id)
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
        Color = color;

        StartDate = startDate;
        DueDate = dueDate;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new EpicCreatedDomainEvent(Id));
    }

    private EpicAggregate()
        : base(EpicId.Empty())
    {
    }

    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string Color { get; private set; } = "#7E57C2";

    public DateTime? StartDate { get; private set; }

    public DateTime? DueDate { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public static EpicAggregate Create(
        Guid projectId,
        string name,
        string? description,
        string color,
        DateTime? startDate,
        DateTime? dueDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(color);

        return new EpicAggregate(
            EpicId.New(),
            projectId,
            name.Trim(),
            description?.Trim(),
            color.Trim(),
            startDate,
            dueDate);
    }

    public void Update(
        string name,
        string? description,
        string color,
        DateTime? startDate,
        DateTime? dueDate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(color);

        Name = name.Trim();
        Description = description?.Trim();
        Color = color.Trim();

        StartDate = startDate;
        DueDate = dueDate;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new EpicUpdatedDomainEvent(Id));
    }

    public void Rename(
        string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (Name.Equals(
            name.Trim(),
            StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        Name = name.Trim();

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new EpicUpdatedDomainEvent(Id));
    }

    public void ChangeColor(
        string color)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(color);

        if (Color == color.Trim())
        {
            return;
        }

        Color = color.Trim();

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new EpicUpdatedDomainEvent(Id));
    }

    public void Delete()
    {
        if (IsDeleted)
        {
            return;
        }

        IsDeleted = true;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new EpicDeletedDomainEvent(Id));
    }

    public void Restore()
    {
        if (!IsDeleted)
        {
            return;
        }

        IsDeleted = false;

        UpdatedOnUtc = DateTime.UtcNow;
    }
}
