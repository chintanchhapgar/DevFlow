namespace DevFlow.SharedKernel.Exceptions;

/// <summary>
/// Base exception for domain-level invariant violations.
/// These represent programming errors (invariant violation), not expected business failures.
/// Expected business failures should use the Result pattern instead.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}
