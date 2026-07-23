using DevFlow.Authentication.Users;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.RefreshTokens;

/// <summary>
/// Represents a refresh token issued to a user.
/// </summary>
public sealed class RefreshToken : Entity<RefreshTokenId>
{
    private RefreshToken(
        RefreshTokenId id,
        UserId userId,
        string token,
        DateTime expiresOnUtc)
        : base(id)
    {
        UserId = userId;
        Token = token;
        ExpiresOnUtc = expiresOnUtc;
        Status = RefreshTokenStatus.Active;
        CreatedOnUtc = DateTime.UtcNow;
    }

    // EF Core
    private RefreshToken()
    {
    }

    public UserId UserId { get; private set; } = default!;

    public string Token { get; private set; } = string.Empty;

    public RefreshTokenStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime? RevokedOnUtc { get; private set; }

    public bool IsActive =>
        Status == RefreshTokenStatus.Active &&
        ExpiresOnUtc > DateTime.UtcNow;

    public static RefreshToken Create(
        UserId userId,
        string token,
        DateTime expiresOnUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        return new RefreshToken(
            RefreshTokenId.New(),
            userId,
            token,
            expiresOnUtc);
    }

    public void Revoke()
    {
        if (Status != RefreshTokenStatus.Active)
        {
            return;
        }

        Status = RefreshTokenStatus.Revoked;
        RevokedOnUtc = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status != RefreshTokenStatus.Active)
        {
            return;
        }

        Status = RefreshTokenStatus.Expired;
    }
}
