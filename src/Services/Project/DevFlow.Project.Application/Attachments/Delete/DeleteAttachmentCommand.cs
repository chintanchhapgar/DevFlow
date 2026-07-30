using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.Delete;

public sealed record DeleteAttachmentCommand(
    Guid AttachmentId)
    : IRequest<Result<DeleteAttachmentResponse>>;
