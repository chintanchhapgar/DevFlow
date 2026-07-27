using DevFlow.Project.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class ProjectAggregateConfiguration
    : IEntityTypeConfiguration<ProjectAggregate>
{
    public void Configure(
        EntityTypeBuilder<ProjectAggregate> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder.Property(x => x.Key)
            .HasMaxLength(10)
            .IsRequired();

        builder.HasIndex(x => x.Key)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.OwnerId)
            .IsRequired();

        builder.Property(x => x.Visibility)
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Metadata
            .FindNavigation(nameof(ProjectAggregate.Members))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(
            x => x.Members,
            members =>
            {
                members.ToTable("ProjectMembers");

                members.WithOwner()
                    .HasForeignKey("ProjectId");

                members.HasKey(
                    "ProjectId",
                    nameof(ProjectMember.UserId));

                members.Property(x => x.Role)
                    .HasConversion<int>();

                members.Property(x => x.JoinedOnUtc);
            });
    }
}
