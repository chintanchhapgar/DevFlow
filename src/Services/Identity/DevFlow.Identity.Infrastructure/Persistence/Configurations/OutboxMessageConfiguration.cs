using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Identity.Infrastructure.Persistence.Configurations;

internal sealed class OutboxMessageConfiguration
    : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages", "identity");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(o => o.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(o => o.CreatedOnUtc)
            .HasColumnName("created_on_utc");

        builder.Property(o => o.ProcessedOnUtc)
            .HasColumnName("processed_on_utc");

        builder.Property(o => o.Error)
            .HasColumnName("error")
            .HasMaxLength(2000);

        builder.Property(o => o.RetryCount)
            .HasColumnName("retry_count")
            .HasDefaultValue(0);

        builder.HasIndex(o => o.ProcessedOnUtc)
            .HasFilter("processed_on_utc IS NULL")
            .HasDatabaseName("ix_identity_outbox_unprocessed");
    }
}
