using DevFlow.Project.Domain.Labels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class LabelConfiguration
    : IEntityTypeConfiguration<Label>
{
    public void Configure(
        EntityTypeBuilder<Label> builder)
    {
        builder.ToTable("Labels");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder.Property(x => x.ProjectId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Color)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc);

        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.Name
        })
        .IsUnique();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
