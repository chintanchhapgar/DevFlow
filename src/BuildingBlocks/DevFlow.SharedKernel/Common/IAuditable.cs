namespace DevFlow.SharedKernel.Abstractions;

/// <summary>
/// Marks an entity as auditable with created/modified tracking.
/// </summary>
public interface IAuditable
{
    DateTime CreatedOnUtc { get; }
    DateTime? ModifiedOnUtc { get; }
}
