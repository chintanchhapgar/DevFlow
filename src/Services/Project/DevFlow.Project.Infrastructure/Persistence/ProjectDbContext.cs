using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Entities;
using DevFlow.Project.Domain.Sprints.Entities;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Infrastructure.Persistence.Configurations;
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

    public DbSet<ProjectAggregate> Projects => Set<ProjectAggregate>();

    public DbSet<WorkItemAggregate> WorkItems => Set<WorkItemAggregate>();

    public DbSet<SprintAggregate> Sprints => Set<SprintAggregate>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("project");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProjectDbContext).Assembly);

        modelBuilder.ApplyConfiguration(
            new SprintAggregateConfiguration());

        base.OnModelCreating(modelBuilder);
    }

}
