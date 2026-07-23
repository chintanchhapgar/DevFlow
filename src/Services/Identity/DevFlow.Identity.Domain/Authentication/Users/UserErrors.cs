using DevFlow.SharedKernel.Results;

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

    
}
