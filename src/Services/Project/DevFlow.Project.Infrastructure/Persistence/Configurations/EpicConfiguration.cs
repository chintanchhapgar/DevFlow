using DevFlow.Project.Domain.Epics.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class EpicConfiguration
    : IEntityTypeConfiguration<EpicAggregate>
{
    public void Configure(
        EntityTypeBuilder<EpicAggregate> builder)
    {
        builder.ToTable("Epics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder.Property(x => x.ProjectId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(4000);

        builder.Property(x => x.Color)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.StartDate);

        builder.Property(x => x.DueDate);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.Name
        })
        .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
