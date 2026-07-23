using DevFlow.SharedKernel.Domain;

namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Base class for aggregate roots.
/// </summary>
/// <typeparam name="TId">Aggregate identifier type.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id)
        : base(id)
    {
    }

    // Required by EF Core
    protected AggregateRoot()
    {
    }
}
