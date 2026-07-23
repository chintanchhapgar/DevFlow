

using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Domain.Authentication.PasswordResetTokens;

public sealed class PasswordResetToken
    : Entity<PasswordResetTokenId>
{
    private PasswordResetToken(
        PasswordResetTokenId id,
        UserId userId,
        string token,
        DateTime expiresOnUtc)
        : base(id)
    {
        UserId = userId;
        Token = token;
        ExpiresOnUtc = expiresOnUtc;
        CreatedOnUtc = DateTime.UtcNow;
        Status = PasswordResetTokenStatus.Active;

        RaiseDomainEvent(
            new PasswordResetRequestedDomainEvent(
                userId,
                id));
    }

    private PasswordResetToken()
    {
    }

    public UserId UserId { get; private set; } = default!;

    public string Token { get; private set; } = string.Empty;

    public PasswordResetTokenStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime? UsedOnUtc { get; private set; }

    public bool IsActive =>
        Status == PasswordResetTokenStatus.Active &&
        ExpiresOnUtc > DateTime.UtcNow;

    public static PasswordResetToken Create(
        UserId userId,
        string token,
        DateTime expiresOnUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        return new PasswordResetToken(
            PasswordResetTokenId.New(),
            userId,
            token,
            expiresOnUtc);
    }

    public void MarkAsUsed()
    {
        if (!IsActive)
            return;

        Status = PasswordResetTokenStatus.Used;
        UsedOnUtc = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status != PasswordResetTokenStatus.Active)
            return;

        Status = PasswordResetTokenStatus.Expired;
    }
}
