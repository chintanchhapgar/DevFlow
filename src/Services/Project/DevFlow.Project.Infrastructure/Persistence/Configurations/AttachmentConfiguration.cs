using DevFlow.Project.Domain.Attachments.Entities;
using DevFlow.Project.Domain.Attachments.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Project.Infrastructure.Persistence.Configurations;

internal sealed class AttachmentConfiguration
    : IEntityTypeConfiguration<Attachment>
{
    public void Configure(
        EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("Attachments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new AttachmentId(value));

        builder.Property(x => x.WorkItemId)
            .IsRequired();

        builder.Property(x => x.UploadedBy)
            .IsRequired();

        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(260)
            .IsRequired();

        builder.Property(x => x.StoredFileName)
            .HasMaxLength(260)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Extension)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.StoragePath)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.SizeInBytes)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc);

        builder.Property(x => x.DeletedOnUtc);

        builder.Property(x => x.DeletedBy);

        builder.HasIndex(x => x.WorkItemId);

        builder.HasIndex(x => new
        {
            x.WorkItemId,
            x.CreatedOnUtc
        });
    }
}
