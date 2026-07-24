using DevFlow.SharedKernel.Domain;

namespace DevFlow.Identity.Domain.Authentication.Users.Owned;

/// <summary>
/// Represents a one-time recovery code owned by a User.
/// </summary>
public sealed class RecoveryCode
{
    private RecoveryCode()
    {
    }

    private RecoveryCode(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Recovery code value.B
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Indicates whether this recovery code has been consumed.
    /// </summary>
    public bool IsUsed { get; private set; }

    /// <summary>
    /// UTC timestamp when the code was used.
    /// </summary>
    public DateTime? UsedOnUtc { get; private set; }

    public static RecoveryCode Create(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return new RecoveryCode(code);
    }

    public bool Matches(string code)
    {
        return string.Equals(
            Code,
            code,
            StringComparison.OrdinalIgnoreCase);
    }

    public Result TryUse()
    {
        if (IsUsed)
        {
            return Result.Failure(
                MultiFactorErrors.RecoveryCodeAlreadyUsed);
        }

        IsUsed = true;
        UsedOnUtc = DateTime.UtcNow;

        return Result.Success();
    }
}
