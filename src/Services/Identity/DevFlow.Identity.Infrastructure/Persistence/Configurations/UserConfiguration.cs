using DevFlow.Identity.Domain.Authentication.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Identity.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF configuration for User.
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new UserId(value));

        builder.Property(x => x.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Role)
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.EmailVerified)
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc);

        builder.Property(x => x.UpdatedOnUtc);

        // ------------------------------------------------------
        // Email Verification
        // ------------------------------------------------------

        builder.Property(x => x.EmailVerificationToken)
            .HasColumnName("EmailVerificationToken");

        builder.Property(x => x.EmailVerificationTokenExpiresOnUtc)
            .HasColumnName("EmailVerificationTokenExpiresOnUtc");

        builder.HasIndex(x => x.EmailVerificationToken)
            .IsUnique();

        // ------------------------------------------------------
        // Computed properties
        // ------------------------------------------------------

        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.FullName);
        builder.Ignore(x => x.IsTwoFactorEnabled);
        builder.Ignore(x => x.IsTwoFactorSetupPending);
        builder.Ignore(x => x.TwoFactorSecret);
        builder.Ignore(x => x.TwoFactorEnabledOnUtc);

        // ------------------------------------------------------
        // Refresh Tokens
        // ------------------------------------------------------

        builder.HasMany(x => x.RefreshTokens)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        // ------------------------------------------------------
        // Multi Factor (Owned)
        // ------------------------------------------------------

        builder.OwnsOne(
            x => x.MultiFactor,
            multiFactor =>
            {
                multiFactor.Property(x => x.Enabled)
                    .HasColumnName("TwoFactorEnabled");

                multiFactor.Property(x => x.Pending)
                    .HasColumnName("TwoFactorPending");

                multiFactor.Property(x => x.Secret)
                    .HasColumnName("TwoFactorSecret")
                    .HasMaxLength(256);

                multiFactor.Property(x => x.EnabledOnUtc)
                    .HasColumnName("TwoFactorEnabledOnUtc");

                multiFactor.Ignore(x => x.IsDisabled);

                multiFactor.OwnsMany(
                    x => x.RecoveryCodes,
                    recovery =>
                    {
                        recovery.ToTable("UserRecoveryCodes");

                        recovery.WithOwner()
                            .HasForeignKey("UserId");

                        recovery.Property<int>("RecoveryCodeId");

                        recovery.HasKey("RecoveryCodeId");

                        recovery.Property(x => x.Code)
                            .HasMaxLength(64)
                            .IsRequired();

                        recovery.Property(x => x.IsUsed);

                        recovery.Property(x => x.UsedOnUtc);
                    });
            });
    }
}
