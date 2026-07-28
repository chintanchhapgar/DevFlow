using DevFlow.SharedKernel.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public static readonly AppError Forbidden =
        new(
            "Project.Forbidden",
            "You are not allowed to modify this project.",
            ErrorType.Forbidden);

    public static readonly AppError AlreadyArchived =
        new(
            "Project.AlreadyArchived",
            "Project is already archived.",
            ErrorType.Validation);

    public static readonly AppError AlreadyActive =
        new(
            "Project.AlreadyActive",
            "Project is already active.",
            ErrorType.Validation);

    public static readonly AppError UserNotFound =
        new(
            "Project.UserNotFound",
            "User not found.",
            ErrorType.Validation);

    public static readonly AppError OwnerCannotBeRemoved =
        new(
            "Project.OwnerCannotBeRemoved",
            "Project owner cannot be removed.",
            ErrorType.Validation);

    public static readonly AppError InvitationNotFound =
        AppError.NotFound(
            "Project.Invitation.NotFound",
            "Invitation was not found.");

    public static readonly AppError InvitationAlreadyExists =
        AppError.Conflict(
            "Project.Invitation.Exists",
            "A pending invitation already exists for this email.");

    public static readonly AppError InvitationExpired =
        AppError.Validation(
            "Project.InvitationExpired",
            "Invitation has expired.");

    public static readonly AppError InvitationAlreadyProcessed =
        AppError.Conflict(
            "Project.InvitationAlreadyProcessed",
            "Invitation has already been processed.");



}
