namespace DevFlow.Project.Application.Attachments.Download;

public sealed record DownloadAttachmentResponse(
    Stream Content,
    string ContentType,
    string FileName);
