using DevFlow.Identity.Application.Common.Abstractions.Persistence;

namespace DevFlow.Identity.Infrastructure.Persistence;

/// <summary>
/// Coordinates persistence operations.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IdentityDbContext _context;

    public UnitOfWork(IdentityDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
