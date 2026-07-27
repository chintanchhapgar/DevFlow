using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence;

public sealed class ProjectDbContext
    : DbContext, IUnitOfWork
{
    public ProjectDbContext(
        DbContextOptions<ProjectDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProjectAggregate> Projects =>
        Set<ProjectAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("project");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProjectDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

}
