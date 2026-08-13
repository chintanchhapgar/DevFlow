using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Storage;
using DevFlow.Project.Domain.Attachments.Errors;
using DevFlow.Project.Domain.Attachments.Repositories;
using DevFlow.Project.Domain.Attachments.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.Delete;

internal sealed class DeleteAttachmentCommandHandler
    : IRequestHandler<
        DeleteAttachmentCommand,
        Result<DeleteAttachmentResponse>>
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAttachmentCommandHandler(
        IAttachmentRepository attachmentRepository,
        IFileStorage fileStorage,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _attachmentRepository = attachmentRepository;
        _fileStorage = fileStorage;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteAttachmentResponse>> Handle(
            DeleteAttachmentCommand request,
            CancellationToken cancellationToken)
    {
        var attachment =
            await _attachmentRepository.GetByIdAsync(
                new AttachmentId(request.AttachmentId),
                cancellationToken);

        if (attachment is null)
        {
            return Result.Failure<DeleteAttachmentResponse>(
                AttachmentErrors.NotFound);
        }

        if (attachment.IsDeleted)
        {
            return Result.Failure<DeleteAttachmentResponse>(
                AttachmentErrors.AlreadyDeleted);
        }

        attachment.Delete(_currentUser.UserId);

        if (await _fileStorage.ExistsAsync(
                attachment.StoragePath,
                cancellationToken))
        {
            await _fileStorage.DeleteAsync(
                attachment.StoragePath,
                cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeleteAttachmentResponse(
                attachment.Id.Value,
                attachment.WorkItemId,
                attachment.OriginalFileName,
                DateTime.UtcNow),
            "Attachment deleted successfully.");
    }
}
