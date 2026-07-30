using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Epics.Errors;

public static class EpicErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Epics.NotFound",
            "Epic was not found.");

    public static readonly AppError AlreadyDeleted =
        AppError.Conflict(
            "Epics.AlreadyDeleted",
            "Epic has already been deleted.");

    public static readonly AppError DuplicateName =
        AppError.Conflict(
            "Epics.DuplicateName",
            "An epic with the same name already exists.");
}
