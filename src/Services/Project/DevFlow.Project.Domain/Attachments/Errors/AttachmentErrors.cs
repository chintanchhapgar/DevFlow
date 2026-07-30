using DevFlow.SharedKernel.Results;

namespace DevFlow.Project.Domain.Attachments.Errors;

public static class AttachmentErrors
{
    public static readonly AppError NotFound =
        AppError.NotFound(
            "Attachments.NotFound",
            "Attachment was not found.");

    public static readonly AppError Forbidden =
        AppError.Forbidden(
            "Attachments.Forbidden",
            "You are not allowed to access this attachment.");

    public static readonly AppError AlreadyDeleted =
        AppError.Conflict(
            "Attachments.AlreadyDeleted",
            "Attachment has already been deleted.");

    public static readonly AppError InvalidFile =
        AppError.Validation(
            "Attachments.InvalidFile",
            "The uploaded file is invalid.");

    public static readonly AppError FileTooLarge =
        AppError.Validation(
            "Attachments.FileTooLarge",
            "The uploaded file exceeds the maximum allowed size.");

    public static readonly AppError UnsupportedFileType =
        AppError.Validation(
            "Attachments.UnsupportedFileType",
            "The uploaded file type is not supported.");
}
