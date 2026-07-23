using DevFlow.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Automatically sets CreatedOnUtc and ModifiedOnUtc for auditable entities.
/// </summary>
public sealed class AuditableInterceptor : SaveChangesInterceptor
{
    private readonly IClock _clock;

    public AuditableInterceptor(IClock clock)
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
            UpdateAuditFields(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext context)
    {
        var now = _clock.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditable.CreatedOnUtc)).CurrentValue = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditable.ModifiedOnUtc)).CurrentValue = now;
            }
        }
    }
}
