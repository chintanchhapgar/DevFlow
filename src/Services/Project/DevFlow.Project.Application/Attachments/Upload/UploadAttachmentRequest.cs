using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.Project.Application.Attachments.Upload;

public sealed class UploadAttachmentRequest
{
    public IFormFile File { get; init; } = default!;
}
