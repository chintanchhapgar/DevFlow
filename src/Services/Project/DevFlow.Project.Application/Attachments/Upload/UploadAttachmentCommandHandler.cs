using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Storage;
using DevFlow.Project.Domain.Attachments.Entities;
using DevFlow.Project.Domain.Attachments.Repositories;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.Upload;

internal sealed class UploadAttachmentCommandHandler
    : IRequestHandler<
        UploadAttachmentCommand,
        Result<UploadAttachmentResponse>>
{
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IFileStorage _fileStorage;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public UploadAttachmentCommandHandler(
        IAttachmentRepository attachmentRepository,
        IWorkItemRepository workItemRepository,
        IFileStorage fileStorage,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _attachmentRepository = attachmentRepository;
        _workItemRepository = workItemRepository;
        _fileStorage = fileStorage;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UploadAttachmentResponse>> Handle(
        UploadAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _workItemRepository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<UploadAttachmentResponse>(
                WorkItemErrors.NotFound);
        }

        var folder =
            $"workitems/{request.WorkItemId}";

        var storagePath =
            await _fileStorage.SaveAsync(
                request.File,
                folder,
                cancellationToken);

        var attachment =
            Attachment.Create(
                request.WorkItemId,
                _currentUser.UserId,
                request.File.FileName,
                Path.GetFileName(storagePath),
                request.File.ContentType,
                Path.GetExtension(request.File.FileName),
                request.File.Length,
                storagePath);

        await _attachmentRepository.AddAsync(
            attachment,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UploadAttachmentResponse(
                attachment.Id.Value,
                attachment.WorkItemId,
                attachment.OriginalFileName,
                attachment.ContentType,
                attachment.SizeInBytes,
                attachment.CreatedOnUtc),
            "Attachment uploaded successfully.");
    }
}
