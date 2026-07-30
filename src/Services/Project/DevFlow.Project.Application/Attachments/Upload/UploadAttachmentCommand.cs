using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DevFlow.Project.Application.Attachments.Upload;

public sealed record UploadAttachmentCommand(
    Guid WorkItemId,
    IFormFile File)
    : IRequest<Result<UploadAttachmentResponse>>;
