using DevFlow.Authentication.Users;
using DevFlow.Identity.Domain.Authentication.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFlow.Identity.Infrastructure.Persistence.Configurations;

internal sealed class RefreshTokenConfiguration
    : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new RefreshTokenId(value));

        builder.Property(x => x.UserId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value));

        builder.Property(x => x.Token)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.CreatedOnUtc);

        builder.Property(x => x.ExpiresOnUtc);

        builder.Property(x => x.RevokedOnUtc);

        builder.HasOne<User>()
            .WithMany(x => x.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
