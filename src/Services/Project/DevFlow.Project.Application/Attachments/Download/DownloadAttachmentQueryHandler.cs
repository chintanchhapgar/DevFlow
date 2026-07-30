using DevFlow.Project.Application.Common.Abstractions.Storage;
using DevFlow.Project.Domain.Attachments.Errors;
using DevFlow.Project.Domain.Attachments.Repositories;
using DevFlow.Project.Domain.Attachments.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.Download;

internal sealed class DownloadAttachmentQueryHandler
    : IRequestHandler<
        DownloadAttachmentQuery,
        Result<DownloadAttachmentResponse>>
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IFileStorage _fileStorage;

    public DownloadAttachmentQueryHandler(
        IAttachmentRepository attachmentRepository,
        IFileStorage fileStorage)
    {
        _attachmentRepository = attachmentRepository;
        _fileStorage = fileStorage;
    }

    public async Task<Result<DownloadAttachmentResponse>> Handle(
        DownloadAttachmentQuery request,
        CancellationToken cancellationToken)
    {
        var attachment =
            await _attachmentRepository.GetByIdAsync(
                new AttachmentId(request.AttachmentId),
                cancellationToken);

        if (attachment is null || attachment.IsDeleted)
        {
            return Result.Failure<DownloadAttachmentResponse>(
                AttachmentErrors.NotFound);
        }

        if (!await _fileStorage.ExistsAsync(
            attachment.StoragePath,
            cancellationToken))
        {
            return Result.Failure<DownloadAttachmentResponse>(
                AttachmentErrors.NotFound);
        }

        var stream =
            await _fileStorage.OpenReadAsync(
                attachment.StoragePath,
                cancellationToken);

        return Result.Success(
            new DownloadAttachmentResponse(
                stream,
                attachment.ContentType,
                attachment.OriginalFileName),
            "Attachment downloaded successfully.");
    }
}
