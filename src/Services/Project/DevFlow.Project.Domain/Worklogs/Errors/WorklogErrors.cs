
using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Worklogs.Errors;

public static class WorklogErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Worklogs.NotFound",
            "Worklog was not found.");

    public static readonly AppError AlreadyStopped =
        AppError.Conflict(
            "Worklogs.AlreadyStopped",
            "Worklog has already been stopped.");

    public static readonly AppError AlreadyDeleted =
        AppError.Conflict(
            "Worklogs.AlreadyDeleted",
            "Worklog has already been deleted.");
}
