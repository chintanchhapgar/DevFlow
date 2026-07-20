using DevFlow.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Intercepts Delete operations on ISoftDelete entities and converts them to updates.
/// </summary>
public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly IClock _clock;

    public SoftDeleteInterceptor(IClock clock)
    {
        _clock = clock;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            HandleSoftDeletes(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleSoftDeletes(DbContext context)
    {
        var softDeleteEntries = context.ChangeTracker
            .Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Deleted)
            .ToList();

        foreach (var entry in softDeleteEntries)
        {
            entry.State = EntityState.Modified;
            entry.Property(nameof(ISoftDelete.IsDeleted)).CurrentValue = true;
            entry.Property(nameof(ISoftDelete.DeletedOnUtc)).CurrentValue = _clock.UtcNow;
        }
    }
}
