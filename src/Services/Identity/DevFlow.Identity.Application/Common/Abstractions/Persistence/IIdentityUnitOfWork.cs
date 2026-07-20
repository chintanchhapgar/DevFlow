namespace DevFlow.Identity.Application.Common.Abstractions.Persistence;

/// <summary>
/// Unit of work for the Identity service.
/// Wraps the transaction boundary for application commands.
/// </summary>
public interface IIdentityUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
