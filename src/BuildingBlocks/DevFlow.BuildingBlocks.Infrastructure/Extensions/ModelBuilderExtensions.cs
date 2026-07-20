using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.BuildingBlocks.Infrastructure.Persistence.Extensions;

/// <summary>
/// EF Core ModelBuilder extensions for common infrastructure configurations.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies the OutboxMessage configuration to the model.
    /// Call this in OnModelCreating of each service DbContext.
    /// </summary>
    public static ModelBuilder ApplyOutboxConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        return modelBuilder;
    }

    /// <summary>
    /// Applies all configurations from a given assembly.
    /// </summary>
    public static ModelBuilder ApplyConfigurationsFromAssembly<T>(
        this ModelBuilder modelBuilder)
    {
        return modelBuilder.ApplyConfigurationsFromAssembly(typeof(T).Assembly);
    }
}
