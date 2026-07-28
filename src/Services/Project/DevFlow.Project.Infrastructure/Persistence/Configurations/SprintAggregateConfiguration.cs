using DevFlow.Project.Domain.Sprints.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class SprintAggregateConfiguration
    : IEntityTypeConfiguration<SprintAggregate>
{
    public void Configure(
        EntityTypeBuilder<SprintAggregate> builder)
    {
        builder.ToTable("Sprints");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder.Property(x => x.ProjectId)
            .IsRequired();

        builder.HasIndex(x => x.ProjectId);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Goal)
            .HasMaxLength(1000);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.StartDate);

        builder.Property(x => x.EndDate);

        builder.Property(x => x.StartedOnUtc);

        builder.Property(x => x.CompletedOnUtc);

        builder.Property(x => x.IsDeleted);

        builder.Property(x => x.CreatedOnUtc);

        builder.Property(x => x.UpdatedOnUtc);

        builder.HasIndex(
            x => new
            {
                x.ProjectId,
                x.Name
            });
    }
}
