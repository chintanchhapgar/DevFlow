using DevFlow.Identity.Domain.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Identity.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        // Strongly-typed ID conversion
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(User.EmailMaxLength)
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(User.FirstNameMaxLength)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(User.LastNameMaxLength)
            .IsRequired();

        builder.Property(u => u.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(User.PasswordHashMaxLength)
            .IsRequired();

        builder.Property(u => u.RefreshToken)
            .HasColumnName("refresh_token")
            .HasMaxLength(500);

        builder.Property(u => u.RefreshTokenExpiresAtUtc)
            .HasColumnName("refresh_token_expires_at_utc");

        builder.Property(u => u.IsEmailVerified)
            .HasColumnName("is_email_verified")
            .HasDefaultValue(false);

        builder.Property(u => u.FailedLoginAttempts)
            .HasColumnName("failed_login_attempts")
            .HasDefaultValue(0);

        builder.Property(u => u.IsLockedOut)
            .HasColumnName("is_locked_out")
            .HasDefaultValue(false);

        builder.Property(u => u.LockedOutUntilUtc)
            .HasColumnName("locked_out_until_utc");

        builder.Property(u => u.LastLoginAtUtc)
            .HasColumnName("last_login_at_utc");

        // IAuditable
        builder.Property(u => u.CreatedOnUtc)
            .HasColumnName("created_on_utc")
            .IsRequired();

        builder.Property(u => u.ModifiedOnUtc)
            .HasColumnName("modified_on_utc");

        // ISoftDelete
        builder.Property(u => u.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.DeletedOnUtc)
            .HasColumnName("deleted_on_utc");

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("ix_users_email");

        builder.HasIndex(u => u.IsDeleted)
            .HasFilter("is_deleted = false")
            .HasDatabaseName("ix_users_active");

        // Global query filter: exclude soft-deleted users
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
