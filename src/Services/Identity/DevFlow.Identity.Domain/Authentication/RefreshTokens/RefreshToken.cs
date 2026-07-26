

using DevFlow.Identity.Domain.Authentication.Users;

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
        DateTime expiresOnUtc,
        string? deviceName,
        string? browser,
        string? operatingSystem,
        string? ipAddress,
        string? userAgent,
        Guid sessionId)
        : base(id)
    {
        UserId = userId;
        Token = token;
        ExpiresOnUtc = expiresOnUtc;
        Status = RefreshTokenStatus.Active;
        CreatedOnUtc = DateTime.UtcNow;
        DeviceName = deviceName;
        Browser = browser;
        OperatingSystem = operatingSystem;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        SessionId = sessionId;
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

    public string? ReplacedByToken { get; private set; }

    public string? RevokedReason { get; private set; }

    public string? DeviceName { get; private set; }

    public string? Browser { get; private set; }

    public string? OperatingSystem { get; private set; }

    public string? IpAddress { get; private set; }

    public string? UserAgent { get; private set; }

    public DateTime? LastUsedOnUtc { get; private set; }

    public Guid SessionId { get; private set; }
    public bool IsActive =>
        Status == RefreshTokenStatus.Active &&
        ExpiresOnUtc > DateTime.UtcNow;

    public static RefreshToken Create(
         UserId userId,
         string token,
         DateTime expiresOnUtc,
         Guid sessionId,
         string? deviceName,
         string? browser,
         string? operatingSystem,
         string? ipAddress,
         string? userAgent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        return new RefreshToken(
            RefreshTokenId.New(),
            userId,
            token,
            expiresOnUtc,
            deviceName,
            browser,
            operatingSystem,
            ipAddress,
            userAgent,
            sessionId);
    }

    public void Revoke(
    string? replacedByToken = null,
    string? reason = null)
    {
        if (Status != RefreshTokenStatus.Active)
        {
            return;
        }

        Status = RefreshTokenStatus.Revoked;
        RevokedOnUtc = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
        RevokedReason = reason;
    }

    public void Expire()
    {
        if (Status != RefreshTokenStatus.Active)
        {
            return;
        }

        Status = RefreshTokenStatus.Expired;
    }

    public void MarkAsUsed()
    {
        LastUsedOnUtc = DateTime.UtcNow;
    }
}
