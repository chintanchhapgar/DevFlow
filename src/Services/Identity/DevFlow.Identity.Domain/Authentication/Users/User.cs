using DevFlow.Identity.Domain.Authentication.RefreshTokens;
using DevFlow.Identity.Domain.Authentication.Users.Owned;
using DevFlow.Identity.Domain.Authentication.Users.ValueObjects;
using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Domain.Authentication.Users;

/// <summary>
/// Represents a system user.
/// Aggregate Root for authentication.
/// </summary>
public sealed partial class User : AggregateRoot<UserId>
{
    private readonly List<RefreshToken> _refreshTokens = [];

    private User(
        UserId id,
        string email,
        string passwordHash,
        string firstName,
        string lastName)
        : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;

        Role = UserRole.Member;
        Status = UserStatus.Active;
        EmailConfirmed = true;

        CreatedOnUtc = DateTime.UtcNow;

        MultiFactor = MultiFactorSettings.Disabled();
    }

    // EF Core
    private User()
    {
        MultiFactor = MultiFactorSettings.Disabled();
    }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public UserStatus Status { get; private set; }

    public bool EmailConfirmed { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public bool IsActive => Status == UserStatus.Active;

    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Multi-factor authentication settings.
    /// EF Core Owned Type.
    /// </summary>
    public MultiFactorSettings MultiFactor { get; private set; }

    public bool IsTwoFactorEnabled => MultiFactor.Enabled;

    public bool IsTwoFactorSetupPending => MultiFactor.Pending;

    public string? TwoFactorSecret => MultiFactor.Secret;

    public DateTime? TwoFactorEnabledOnUtc =>
        MultiFactor.EnabledOnUtc;

    public IReadOnlyCollection<RefreshToken> RefreshTokens =>
        _refreshTokens.AsReadOnly();


    public int AccessFailedCount { get; private set; }

    public DateTime? LockoutEndUtc { get; private set; }

    public bool IsLockedOut =>
        LockoutEndUtc.HasValue &&
        LockoutEndUtc > DateTime.UtcNow;

    public static User Create(
        string email,
        string passwordHash,
        string firstName,
        string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        var user = new User(
            UserId.New(),
            email.Trim().ToLowerInvariant(),
            passwordHash,
            firstName.Trim(),
            lastName.Trim());

        user.RaiseDomainEvent(
            new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        Status = UserStatus.Active;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ChangePassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void UpdateProfile(
        string firstName,
        string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = UserStatus.Active;

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Disabled;

        UpdatedOnUtc = DateTime.UtcNow;
    }
    /// <summary>
    /// Creates a new refresh token.
    /// </summary>
    public RefreshToken CreateRefreshToken(
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

        var refreshToken = RefreshToken.Create(
            Id,
            token,
            expiresOnUtc,
            sessionId,
            deviceName,
            browser,
            operatingSystem,
            ipAddress,
            userAgent);

        _refreshTokens.Add(refreshToken);

        UpdatedOnUtc = DateTime.UtcNow;

        return refreshToken;
    }

    /// <summary>
    /// Starts the MFA enrollment process by storing a TOTP secret.
    /// </summary>
    public Result BeginTwoFactorSetup(string secret)
    {
        var result = MultiFactor.BeginSetup(secret);

        if (result.IsFailure)
        {
            return result;
        }

        UpdatedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    /// <summary>
    /// Completes MFA enrollment after the verification code
    /// has been successfully validated.
    /// </summary>
    public Result CompleteTwoFactorSetup()
    {
        var result = MultiFactor.CompleteSetup();

        if (result.IsFailure)
        {
            return result;
        }

        UpdatedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    /// <summary>
    /// Disables MFA for the user.
    /// </summary>
    public Result DisableTwoFactor()
    {
        var result = MultiFactor.Disable();

        if (result.IsFailure)
        {
            return result;
        }

        UpdatedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    /// <summary>
    /// Replaces all recovery codes with a newly generated set.
    /// </summary>
    public void ReplaceRecoveryCodes(
        IEnumerable<RecoveryCode> recoveryCodes)
    {
        ArgumentNullException.ThrowIfNull(recoveryCodes);

        MultiFactor.ReplaceRecoveryCodes(recoveryCodes);

        UpdatedOnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Attempts to consume a recovery code.
    /// </summary>
    public Result TryUseRecoveryCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        var recoveryCode = MultiFactor.RecoveryCodes
            .FirstOrDefault(x => x.Matches(code));

        if (recoveryCode is null)
        {
            return Result.Failure(
                MultiFactorErrors.InvalidRecoveryCode);
        }

        var result = recoveryCode.TryUse();

        if (result.IsFailure)
        {
            return result;
        }

        UpdatedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    public void RecordFailedLogin(
    int maxAttempts,
    TimeSpan lockoutDuration)
    {
        AccessFailedCount++;

        if (AccessFailedCount >= maxAttempts)
        {
            LockoutEndUtc =
                DateTime.UtcNow.Add(lockoutDuration);

            AccessFailedCount = 0;
        }

        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void ResetFailedLogin()
    {
        AccessFailedCount = 0;
        LockoutEndUtc = null;

        UpdatedOnUtc = DateTime.UtcNow;
    }
}
