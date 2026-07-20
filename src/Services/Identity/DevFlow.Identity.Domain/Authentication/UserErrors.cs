using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Domain.Authentication;

/// <summary>
/// Domain Errorss for the User aggregate.
/// Centralizing Errorss makes them discoverable and testable.
/// </summary>
public static class UserErrors
{
    public static readonly Errors NotFound = Errors.NotFound(
        "User.NotFound",
        "The user with the specified identifier was not found.");

    public static readonly Errors EmailNotFound = Errors.NotFound(
        "User.EmailNotFound",
        "No user was found with the provided email address.");

    public static readonly Errors EmailAlreadyExists = Errors.Conflict(
        "User.EmailAlreadyExists",
        "A user with this email address already exists.");

    public static readonly Errors InvalidCredentials = Errors.Unauthorized(
        "User.InvalidCredentials",
        "The provided email or password is incorrect.");

    public static readonly Errors InvalidRefreshToken = Errors.Unauthorized(
        "User.InvalidRefreshToken",
        "The provided refresh token is invalid or has expired.");

    public static readonly Errors EmailNotVerified = Errors.Forbidden(
        "User.EmailNotVerified",
        "Email address must be verified before accessing this resource.");

    public static readonly Errors AccountLocked = Errors.Forbidden(
        "User.AccountLocked",
        "The account has been locked due to too many failed login attempts.");

    public static Errors InvalidField(string field, string reason) =>
        Errors.Validation($"User.Invalid{field}", reason);
}
