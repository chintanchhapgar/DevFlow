using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.BuildingBlocks.Infrastructure.Outbox;

/// <summary>
/// EF Core configuration for the OutboxMessage entity.
/// </summary>
public sealed class OutboxMessageConfiguration
    : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(
        EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Content)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.ProcessedOnUtc);

        builder.Property(x => x.Error);

        builder.Property(x => x.RetryCount)
            .IsRequired();
    }
}
