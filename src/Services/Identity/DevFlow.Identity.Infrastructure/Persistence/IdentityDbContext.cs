using DevFlow.Identity.Domain.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Infrastructure.Persistence;

/// <summary>
/// Identity database context.
/// </summary>
public sealed class IdentityDbContext : DbContext
{
    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IdentityDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
