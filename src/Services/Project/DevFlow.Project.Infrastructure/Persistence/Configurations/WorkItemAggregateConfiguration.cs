using DevFlow.Project.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class WorkItemAggregateConfiguration
    : IEntityTypeConfiguration<WorkItemAggregate>
{
    public void Configure(EntityTypeBuilder<WorkItemAggregate> builder)
    {
        builder.ToTable("WorkItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder.Property(x => x.ProjectId)
            .IsRequired();

        builder.Property(x => x.Key)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.Key)
            .IsUnique();

        builder.Property(x => x.Title)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(5000);

        builder.Property(x => x.Type)
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Priority)
            .HasConversion<int>();

        builder.Property(x => x.AssigneeId);

        builder.Property(x => x.ReporterId)
            .IsRequired();

        builder.Property(x => x.EpicId);

        builder.Property(x => x.ParentId);

        builder.Property(x => x.SprintId);

        builder.Property(x => x.EstimateHours)
            .HasPrecision(8, 2);

        builder.Property(x => x.DueDate);

        builder.Property(x => x.IsDeleted);

        builder.Property(x => x.CreatedOnUtc);

        builder.Property(x => x.UpdatedOnUtc);

        builder.HasIndex(x => x.ProjectId);

        builder.Property(x => x.ChildCount)
    .HasDefaultValue(0);

        builder.Metadata
    .FindNavigation(nameof(WorkItemAggregate.Labels))!
    .SetPropertyAccessMode(PropertyAccessMode.Field);


        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.Status
        });

        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.AssigneeId
        });

        builder.HasIndex(x => new
        {
            x.ProjectId,
            x.Priority
        });
    }
}
