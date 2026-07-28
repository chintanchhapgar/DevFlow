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

        builder.OwnsMany(
            x => x.Invitations,
            invitations =>
            {
                invitations.ToTable("ProjectInvitations");

                invitations.WithOwner()
                    .HasForeignKey("ProjectId");

                invitations.UsePropertyAccessMode(PropertyAccessMode.Field);

                invitations.HasKey(x => x.Id);

                invitations.Property(x => x.Id)
                    .ValueGeneratedNever();

                invitations.Property(x => x.Email)
                    .HasMaxLength(256)
                    .IsRequired();

                invitations.Property(x => x.Role)
                    .HasConversion<int>();

                invitations.Property(x => x.Status)
                    .HasConversion<int>();

                invitations.Property(x => x.Token)
                    .IsRequired();

                invitations.HasIndex(x => x.Token)
                    .IsUnique();

                invitations.Property(x => x.InvitedBy)
                    .IsRequired();

                invitations.Property(x => x.InvitedOnUtc);

                invitations.Property(x => x.ExpiresOnUtc);

                invitations.Property(x => x.AcceptedOnUtc);

                invitations.HasIndex(new[]
                {
                    "ProjectId",
                    nameof(ProjectInvitation.Email),
                    nameof(ProjectInvitation.Status)
                });
            });
    }
}
