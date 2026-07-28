using DevFlow.Project.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class ProjectAggregateConfiguration
    : IEntityTypeConfiguration<ProjectAggregate>
{
    public void Configure(EntityTypeBuilder<ProjectAggregate> builder)
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

        // ✅ REMOVED: builder.Metadata.FindNavigation(...).SetPropertyAccessMode(...)
        // This is handled inside OwnsMany via UsePropertyAccessMode

        builder.OwnsMany(
    x => x.Members,
    members =>
    {
        members.ToTable("ProjectMembers");

        members.WithOwner()
            .HasForeignKey("ProjectId");

        members.UsePropertyAccessMode(PropertyAccessMode.Field);

        members.HasKey(x => x.Id);

        members.Property(x => x.Id)
            .ValueGeneratedNever();

        members.HasIndex(
            "ProjectId",
            nameof(ProjectMember.UserId))
            .IsUnique();

        members.Property(x => x.UserId)
            .IsRequired();

        members.Property(x => x.Role)
            .HasConversion<int>();

        members.Property(x => x.JoinedOnUtc);
    });
    }
}
