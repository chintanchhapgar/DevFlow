namespace DevFlow.SharedKernel.Common;

/// <summary>
/// Marks an entity as supporting soft deletion.
/// EF Core global query filters use this to exclude deleted records.
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTime? DeletedOnUtc { get; }
}
