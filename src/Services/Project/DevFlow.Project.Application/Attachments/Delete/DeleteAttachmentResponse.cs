namespace DevFlow.Project.Application.Attachments.Delete;

public sealed record DeleteAttachmentResponse(
    Guid AttachmentId,
    Guid WorkItemId,
    string FileName,
    DateTime DeletedOnUtc);
