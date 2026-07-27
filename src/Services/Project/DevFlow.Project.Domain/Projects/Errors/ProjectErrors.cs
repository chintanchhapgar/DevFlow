using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Projects.Errors;

public static class ProjectErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Project.NotFound",
            "The project was not found.");

    public static readonly AppError DuplicateKey =
        AppError.Conflict(
            "Project.DuplicateKey",
            "Project key already exists.");

    public static readonly AppError InvalidName =
        AppError.Validation(
            "Project.InvalidName",
            "Project name is invalid.");

    public static readonly AppError InvalidKey =
        AppError.Validation(
            "Project.InvalidKey",
            "Project key is invalid.");

    public static readonly AppError Archived =
        AppError.Conflict(
            "Project.Archived",
            "Archived projects cannot be modified.");

    public static readonly AppError MemberAlreadyExists =
        AppError.Conflict(
            "Project.MemberAlreadyExists",
            "User is already a member.");

    public static readonly AppError MemberNotFound =
        AppError.NotFound(
            "Project.MemberNotFound",
            "Member not found.");

    public static readonly AppError CannotRemoveOwner =
        AppError.Conflict(
            "Project.CannotRemoveOwner",
            "Project owner cannot be removed.");
}
