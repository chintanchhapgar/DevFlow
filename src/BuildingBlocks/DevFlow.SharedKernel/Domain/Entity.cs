namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Base class for all domain entities.
/// Entities have identity and lifecycle.
/// Equality is based on identity, not structural comparison.
/// </summary>
/// <typeparam name="TId">The strongly-typed ID type.</typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    protected Entity(TId id)
    {
        Id = id;
    }

    // Required for EF Core
    protected Entity() { }

    public TId Id { get; private set; } = default!;

    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != GetType()) return false;

        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Equals(entity);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}
