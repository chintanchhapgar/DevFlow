using DevFlow.Project.Domain.Worklogs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class WorklogConfiguration
    : IEntityTypeConfiguration<WorklogAggregate>
{
    public void Configure(
        EntityTypeBuilder<WorklogAggregate> builder)
    {
        builder.ToTable("Worklogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder.Property(x => x.WorkItemId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(4000);

        builder.Property(x => x.StartedAtUtc)
            .IsRequired();

        builder.Property(x => x.EndedAtUtc);

        builder.Property(x => x.MinutesSpent)
            .IsRequired();

        builder.Property(x => x.IsRunning)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => x.WorkItemId);

        builder.HasIndex(x => x.UserId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
