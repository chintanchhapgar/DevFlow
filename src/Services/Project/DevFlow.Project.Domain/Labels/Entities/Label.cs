using DevFlow.Project.Domain.Labels.Events;
using DevFlow.Project.Domain.Labels.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Labels.Entities;

public sealed class Label
    : AggregateRoot<LabelId>
{
    private Label(
        LabelId id,
        Guid projectId,
        string name,
        string color)
        : base(id)
    {
        ProjectId = projectId;
        Name = name;
        Color = color;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new LabelCreatedDomainEvent(Id));
    }

    private Label()
        : base(LabelId.Empty())
    {
    }

    public Guid ProjectId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Color { get; private set; } = "#2196F3";

    public bool IsDeleted { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public static Label Create(
        Guid projectId,
        string name,
        string color)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(color);

        return new Label(
            LabelId.New(),
            projectId,
            name.Trim(),
            color.Trim());
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
            new LabelUpdatedDomainEvent(Id));
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
            new LabelUpdatedDomainEvent(Id));
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
            new LabelDeletedDomainEvent(Id));
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
