namespace DevFlow.Project.Application.Attachments.GetAll;

public sealed record GetAllAttachmentsResponse(
    Guid AttachmentId,
    string OriginalFileName,
    string ContentType,
    string Extension,
    long SizeInBytes,
    DateTime CreatedOnUtc,
    Guid UploadedBy);
