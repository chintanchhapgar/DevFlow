using DevFlow.SharedKernel.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevFlow.Identity.Domain.Authentication.Users;

public static class UserErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Users.NotFound",
            "The user was not found.");

    public static readonly AppError EmailAlreadyExists =
        AppError.Conflict(
            "Users.EmailAlreadyExists",
            "Email address is already registered.");

    public static readonly AppError InvalidCredentials =
        AppError.Validation(
            "Users.InvalidCredentials",
            "Invalid email or password.");

    public static readonly AppError EmailNotConfirmed =
    new(
        "Users.EmailNotConfirmed",
        "Email address has not been verified.");

    public static readonly AppError UserInactive =
        new(
            "Users.UserInactive",
            "User account is inactive.");

    public static readonly AppError InvalidRefreshToken =
        AppError.Validation(
            "Users.InvalidRefreshToken",
            "Invalid token.");

    public static readonly AppError InvalidResetToken =
        AppError.NotFound(
            "Users.InvalidResetToken",
            "Password reset token is invalid or expired.");

    public static readonly AppError UserNotFound =
        AppError.NotFound(
            "Users.UserNotFound",
            "User not found.");

    public static readonly AppError InvalidCurrentPassword =
        AppError.Failure(
            "Users.InvalidCurrentPassword",
            "Current password is incorrect.");

    public static readonly AppError InvalidVerificationToken =
    AppError.NotFound(
        "Users.InvalidVerificationToken",
        "The verification token is invalid or has expired.");

    public static readonly AppError EmailAlreadyVerified =
        AppError.Conflict(
            "Users.EmailAlreadyVerified",
            "Email has already been verified.");
}
