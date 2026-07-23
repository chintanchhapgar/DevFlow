namespace DevFlow.Identity.Application.Common.Abstractions.Persistence;

/// <summary>
/// Coordinates persistence changes.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
