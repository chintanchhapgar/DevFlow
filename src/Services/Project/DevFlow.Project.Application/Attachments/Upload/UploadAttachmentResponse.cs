namespace DevFlow.Project.Application.Attachments.Upload;

public sealed record UploadAttachmentResponse(
    Guid AttachmentId,
    Guid WorkItemId,
    string OriginalFileName,
    string ContentType,
    long SizeInBytes,
    DateTime CreatedOnUtc);
