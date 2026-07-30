using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.Download;

public sealed record DownloadAttachmentQuery(
    Guid AttachmentId)
    : IRequest<Result<DownloadAttachmentResponse>>;
