namespace DevFlow.SharedKernel.Abstractions;

/// <summary>
/// Abstraction over system time.
/// Enables deterministic time in tests.
/// </summary>
public interface IClock
{
    DateTime UtcNow { get; }
    DateOnly Today { get; }
}
