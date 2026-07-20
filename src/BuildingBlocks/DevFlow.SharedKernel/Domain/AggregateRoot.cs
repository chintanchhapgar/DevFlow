namespace DevFlow.SharedKernel.Domain;

/// <summary>
/// Base class for aggregate roots.
/// Aggregate roots are the entry points to an aggregate.
/// They manage domain events and enforce invariants.
/// </summary>
/// <typeparam name="TId">The strongly-typed ID type.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot(TId id) : base(id) { }

    // Required for EF Core
    protected AggregateRoot() { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
