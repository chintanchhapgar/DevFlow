
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevFlow.Notification.Domain.Notifications;
namespace DevFlow.Notification.Infrastructure.Persistence.Configurations;

internal sealed class NotificationConfiguration
    : IEntityTypeConfiguration<DevFlow.Notification.Domain.Notifications.Notification>
{
    public void Configure(
        EntityTypeBuilder<DevFlow.Notification.Domain.Notifications.Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new NotificationId(value));

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.CreatedOnUtc);

        builder.Property(x => x.ReadOnUtc);

        builder.Ignore(x => x.DomainEvents);
    }
}
