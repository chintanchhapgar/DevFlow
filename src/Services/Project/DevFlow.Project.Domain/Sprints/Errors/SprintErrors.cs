
using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Sprints.Errors;

public static class SprintErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Sprint.NotFound",
            "Sprint was not found.");

    public static readonly AppError Forbidden =
        AppError.Forbidden(
            "Sprint.Forbidden",
            "You are not allowed to perform this action.");

    public static readonly AppError AlreadyStarted =
        AppError.Conflict(
            "Sprint.AlreadyStarted",
            "Sprint has already started.");

    public static readonly AppError AlreadyCompleted =
        AppError.Conflict(
            "Sprint.AlreadyCompleted",
            "Sprint has already been completed.");

    public static readonly AppError InvalidState =
        AppError.Validation(
            "Sprint.InvalidState",
            "Sprint cannot transition to the requested state.");
}
