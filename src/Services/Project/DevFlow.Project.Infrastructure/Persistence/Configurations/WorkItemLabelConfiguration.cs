using DevFlow.Project.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class WorkItemLabelConfiguration
    : IEntityTypeConfiguration<WorkItemLabel>
{
    public void Configure(
        EntityTypeBuilder<WorkItemLabel> builder)
    {
        builder.ToTable("WorkItemLabels");

        builder.HasKey(x => new
        {
            x.WorkItemId,
            x.LabelId
        });

        builder.Property(x => x.WorkItemId);

        builder.Property(x => x.LabelId);
    }
}
