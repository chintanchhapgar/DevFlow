using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Domain.Authentication.Users;

public static class MultiFactorErrors
{
    public static readonly AppError AlreadyEnabled =
        AppError.Conflict(
            "MultiFactor.AlreadyEnabled",
            "Two-factor authentication is already enabled.");

    public static readonly AppError AlreadyPending =
        AppError.Conflict(
            "MultiFactor.AlreadyPending",
            "Two-factor authentication setup is already in progress.");

    public static readonly AppError NotPending =
        AppError.Validation(
            "MultiFactor.NotPending",
            "Two-factor setup has not been started.");

    public static readonly AppError AlreadyDisabled =
        AppError.Validation(
            "MultiFactor.AlreadyDisabled",
            "Two-factor authentication is already disabled.");

    public static readonly AppError InvalidRecoveryCode =
        AppError.Validation(
            "MultiFactor.InvalidRecoveryCode",
            "The recovery code is invalid.");

    public static readonly AppError RecoveryCodeAlreadyUsed =
        AppError.Validation(
            "MultiFactor.RecoveryCodeAlreadyUsed",
            "The recovery code has already been used.");
}
