using DevFlow.Identity.Domain.Authentication.Users.Owned;
using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Domain.Authentication.Users.ValueObjects;

/// <summary>
/// Represents a user's Multi-Factor Authentication settings.
/// Owned by the User aggregate.
/// </summary>
public sealed class MultiFactorSettings
{
    private readonly List<RecoveryCode> _recoveryCodes = [];

    // Required by EF Core
    private MultiFactorSettings()
    {
    }

    private MultiFactorSettings(
        bool enabled,
        bool pending,
        string? secret,
        DateTime? enabledOnUtc)
    {
        Enabled = enabled;
        Pending = pending;
        Secret = secret;
        EnabledOnUtc = enabledOnUtc;
    }

    /// <summary>
    /// Indicates whether MFA is enabled.
    /// </summary>
    public bool Enabled { get; private set; }

    /// <summary>
    /// Indicates whether setup has started.
    /// </summary>
    public bool Pending { get; private set; }

    /// <summary>
    /// Base32 encoded TOTP secret.
    /// </summary>
    public string? Secret { get; private set; }

    /// <summary>
    /// Date/time MFA was enabled.
    /// </summary>
    public DateTime? EnabledOnUtc { get; private set; }

    /// <summary>
    /// Recovery codes.
    /// </summary>
    public IReadOnlyCollection<RecoveryCode> RecoveryCodes =>
        _recoveryCodes.AsReadOnly();

    public bool IsDisabled => !Enabled && !Pending;

    public static MultiFactorSettings Disabled()
    {
        return new MultiFactorSettings(
            enabled: false,
            pending: false,
            secret: null,
            enabledOnUtc: null);
    }

    public Result BeginSetup(string secret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        if (Enabled)
        {
            return Result.Failure(
                MultiFactorErrors.AlreadyEnabled);
        }

        if (Pending)
        {
            return Result.Failure(
                MultiFactorErrors.AlreadyPending);
        }

        Secret = secret;
        Pending = true;

        return Result.Success();
    }

    public Result CompleteSetup()
    {
        if (!Pending)
        {
            return Result.Failure(
                MultiFactorErrors.NotPending);
        }

        Enabled = true;
        Pending = false;
        EnabledOnUtc = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Disable()
    {
        if (!Enabled)
        {
            return Result.Failure(
                MultiFactorErrors.AlreadyDisabled);
        }

        Enabled = false;
        Pending = false;
        Secret = null;
        EnabledOnUtc = null;

        _recoveryCodes.Clear();

        return Result.Success();
    }

    public void ReplaceRecoveryCodes(
        IEnumerable<RecoveryCode> recoveryCodes)
    {
        ArgumentNullException.ThrowIfNull(recoveryCodes);

        _recoveryCodes.Clear();
        _recoveryCodes.AddRange(recoveryCodes);
    }
}
