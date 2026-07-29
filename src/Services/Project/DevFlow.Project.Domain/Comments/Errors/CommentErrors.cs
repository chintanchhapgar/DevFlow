using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Comments.Errors;

public static class CommentErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Comments.NotFound",
            "Comment was not found.");

    public static readonly AppError Forbidden =
        AppError.Forbidden(
            "Comments.Forbidden",
            "You are not allowed to modify this comment.");

    public static readonly AppError AlreadyDeleted =
        AppError.Conflict(
            "Comments.AlreadyDeleted",
            "Comment has already been deleted.");

    public static readonly AppError EmptyContent =
        AppError.Validation(
            "Comments.EmptyContent",
            "Comment content cannot be empty.");
}
