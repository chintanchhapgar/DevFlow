
using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Labels.Errors;

public static class LabelErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Labels.NotFound",
            "Label was not found.");

    public static readonly AppError AlreadyDeleted =
        AppError.Conflict(
            "Labels.AlreadyDeleted",
            "Label has already been deleted.");

    public static readonly AppError DuplicateName =
        AppError.Conflict(
            "Labels.DuplicateName",
            "A label with the same name already exists.");
}
