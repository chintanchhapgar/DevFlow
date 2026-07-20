namespace DevFlow.SharedKernel.Exceptions;

/// <summary>
/// Thrown when a required aggregate root or entity cannot be found.
/// Generally used in infrastructure layer when loading aggregates by ID.
/// </summary>
public sealed class NotFoundException : DomainException
{
    public NotFoundException(string entityName, object id)
        : base($"{entityName} with ID '{id}' was not found.") { }
}
