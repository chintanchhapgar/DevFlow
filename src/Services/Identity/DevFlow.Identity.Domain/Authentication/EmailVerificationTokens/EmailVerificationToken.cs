using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;

/// <summary>
/// Email verification token.
/// </summary>
public sealed class EmailVerificationToken
    : Entity<EmailVerificationTokenId>
{
    private EmailVerificationToken(
        EmailVerificationTokenId id,
        UserId userId,
        string token,
        DateTime expiresOnUtc)
        : base(id)
    {
        UserId = userId;
        Token = token;
        ExpiresOnUtc = expiresOnUtc;
        CreatedOnUtc = DateTime.UtcNow;
        Status = EmailVerificationStatus.Active;
    }

    // EF Core
    private EmailVerificationToken()
    {
    }

    public UserId UserId { get; private set; } = default!;

    public string Token { get; private set; } = string.Empty;

    public EmailVerificationStatus Status { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public DateTime? UsedOnUtc { get; private set; }

    public bool IsActive =>
        Status == EmailVerificationStatus.Active &&
        ExpiresOnUtc > DateTime.UtcNow;

    public static EmailVerificationToken Create(
        UserId userId,
        string token,
        DateTime expiresOnUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        return new EmailVerificationToken(
            EmailVerificationTokenId.New(),
            userId,
            token,
            expiresOnUtc);
    }

    public void MarkAsUsed()
    {
        if (Status != EmailVerificationStatus.Active)
        {
            return;
        }

        Status = EmailVerificationStatus.Used;
        UsedOnUtc = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status != EmailVerificationStatus.Active)
        {
            return;
        }

        Status = EmailVerificationStatus.Expired;
    }
}
