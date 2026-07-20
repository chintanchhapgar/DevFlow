using DevFlow.Identity.Domain.Authentication.Events;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Domain;
using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Domain.Authentication;

/// <summary>
/// User aggregate root.
/// Owns all identity-related business logic including registration,
/// authentication state, and profile management.
/// </summary>
public sealed class User : AggregateRoot<UserId>, IAuditable, ISoftDelete
{
    // Constraints
    public const int FirstNameMaxLength = 100;
    public const int LastNameMaxLength = 100;
    public const int EmailMaxLength = 256;
    public const int PasswordHashMaxLength = 500;

    // Private backing fields for EF Core
    private string _email = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private string _passwordHash = string.Empty;

    // EF Core constructor
    private User() : base() { }

    private User(
        UserId id,
        string email,
        string firstName,
        string lastName,
        string passwordHash) : base(id)
    {
        _email = email;
        _firstName = firstName;
        _lastName = lastName;
        _passwordHash = passwordHash;
    }

    // ─── Public Properties ───────────────────────────────────────────────────

    public string Email => _email;
    public string FirstName => _firstName;
    public string LastName => _lastName;
    public string PasswordHash => _passwordHash;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAtUtc { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public bool IsLockedOut { get; private set; }
    public DateTime? LockedOutUntilUtc { get; private set; }
    public DateTime? LastLoginAtUtc { get; private set; }

    // IAuditable
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ModifiedOnUtc { get; private set; }

    // ISoftDelete
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }

    // Computed
    public string FullName => $"{_firstName} {_lastName}";

    // ─── Factory Method ──────────────────────────────────────────────────────

    /// <summary>
    /// Creates a new User. This is the only way to create a valid User instance.
    /// Returns a Result to communicate domain validation failures.
    /// </summary>
    public static Result<User> Create(
        string email,
        string firstName,
        string lastName,
        string passwordHash)
    {
        var emailValidation = ValidateEmail(email);
        if (emailValidation.IsFailure) return emailValidation.Error;

        var nameValidation = ValidateName(firstName, lastName);
        if (nameValidation.IsFailure) return nameValidation.Error;

        var user = new User(
            id: UserId.New(),
            email: email.Trim().ToLowerInvariant(),
            firstName: firstName.Trim(),
            lastName: lastName.Trim(),
            passwordHash: passwordHash);

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName));

        return user;
    }

    // ─── Domain Behavior ─────────────────────────────────────────────────────

    /// <summary>
    /// Records a successful login and resets failed attempt counter.
    /// </summary>
    public void RecordLogin(DateTime loginAtUtc)
    {
        FailedLoginAttempts = 0;
        IsLockedOut = false;
        LockedOutUntilUtc = null;
        LastLoginAtUtc = loginAtUtc;

        RaiseDomainEvent(new UserLoggedInDomainEvent(Id, Email, loginAtUtc));
    }

    /// <summary>
    /// Records a failed login attempt. Locks the account after 5 failures.
    /// </summary>
    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;

        if (FailedLoginAttempts >= 5)
        {
            IsLockedOut = true;
            LockedOutUntilUtc = DateTime.UtcNow.AddMinutes(15);
        }
    }

    /// <summary>
    /// Checks if the account is currently locked out.
    /// </summary>
    public bool IsCurrentlyLockedOut(DateTime utcNow)
    {
        if (!IsLockedOut) return false;

        if (LockedOutUntilUtc.HasValue && utcNow > LockedOutUntilUtc.Value)
        {
            // Auto-unlock after lockout period
            IsLockedOut = false;
            LockedOutUntilUtc = null;
            FailedLoginAttempts = 0;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Sets a new refresh token with expiration.
    /// </summary>
    public void SetRefreshToken(string refreshToken, DateTime expiresAtUtc)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiresAtUtc = expiresAtUtc;
    }

    /// <summary>
    /// Validates and consumes the refresh token.
    /// </summary>
    public Result ValidateRefreshToken(string refreshToken, DateTime utcNow)
    {
        if (RefreshToken != refreshToken)
            return UserErrors.InvalidRefreshToken;

        if (!RefreshTokenExpiresAtUtc.HasValue || utcNow > RefreshTokenExpiresAtUtc)
            return UserErrors.InvalidRefreshToken;

        return Result.Success();
    }

    /// <summary>
    /// Revokes the current refresh token (logout).
    /// </summary>
    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAtUtc = null;
    }

    /// <summary>
    /// Verifies the user's email address.
    /// </summary>
    public void VerifyEmail()
    {
        IsEmailVerified = true;
    }

    /// <summary>
    /// Updates the user's profile information.
    /// </summary>
    public Result UpdateProfile(string firstName, string lastName)
    {
        var validation = ValidateName(firstName, lastName);
        if (validation.IsFailure) return validation;

        _firstName = firstName.Trim();
        _lastName = lastName.Trim();

        RaiseDomainEvent(new UserProfileUpdatedDomainEvent(Id, _firstName, _lastName));

        return Result.Success();
    }

    /// <summary>
    /// Updates the password hash (after password change).
    /// </summary>
    public void UpdatePassword(string newPasswordHash)
    {
        _passwordHash = newPasswordHash;
        RevokeRefreshToken(); // Force re-login after password change
    }

    // ─── Private Validation ──────────────────────────────────────────────────

    private static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return UserErrors.InvalidField("Email", "Email cannot be empty.");

        if (email.Length > EmailMaxLength)
            return UserErrors.InvalidField("Email",
                $"Email cannot exceed {EmailMaxLength} characters.");

        if (!email.Contains('@'))
            return UserErrors.InvalidField("Email", "Email must be a valid email address.");

        return Result.Success();
    }

    private static Result ValidateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return UserErrors.InvalidField("FirstName", "First name cannot be empty.");

        if (firstName.Length > FirstNameMaxLength)
            return UserErrors.InvalidField("FirstName",
                $"First name cannot exceed {FirstNameMaxLength} characters.");

        if (string.IsNullOrWhiteSpace(lastName))
            return UserErrors.InvalidField("LastName", "Last name cannot be empty.");

        if (lastName.Length > LastNameMaxLength)
            return UserErrors.InvalidField("LastName",
                $"Last name cannot exceed {LastNameMaxLength} characters.");

        return Result.Success();
    }
}
