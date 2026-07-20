using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication;

/// <summary>
/// Strongly-typed identifier for the User aggregate root.
/// </summary>
public sealed record UserId(Guid Value) : StronglyTypedId<Guid>(Value)
{
    public static UserId New() => new(Guid.NewGuid());

    public static UserId Empty => new(Guid.Empty);

    public static UserId From(Guid value) => new(value);

    /// <summary>
    /// Parses a string representation of a UserId.
    /// Returns null if parsing fails.
    /// </summary>
    public static UserId? TryParse(string? value)
    {
        return Guid.TryParse(value, out var guid) ? new UserId(guid) : null;
    }
}
