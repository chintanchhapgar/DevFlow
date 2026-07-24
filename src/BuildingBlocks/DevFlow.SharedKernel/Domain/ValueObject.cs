using System.Collections;

namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Base class for DDD Value Objects.
/// </summary>
public abstract class ValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(
                0,
                (hash, obj) =>
                {
                    unchecked
                    {
                        return HashCode.Combine(
                            hash,
                            obj?.GetHashCode() ?? 0);
                    }
                });
    }

    public static bool operator ==(
        ValueObject? left,
        ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        ValueObject? left,
        ValueObject? right)
    {
        return !Equals(left, right);
    }
}
