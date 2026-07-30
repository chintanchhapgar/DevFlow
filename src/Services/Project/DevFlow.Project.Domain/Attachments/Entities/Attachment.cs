using DevFlow.Project.Domain.Attachments.Events;
using DevFlow.Project.Domain.Attachments.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Attachments.Entities;

public sealed class Attachment
    : AggregateRoot<AttachmentId>
{
    private Attachment(
        AttachmentId id,
        Guid workItemId,
        Guid uploadedBy,
        string originalFileName,
        string storedFileName,
        string contentType,
        string extension,
        long sizeInBytes,
        string storagePath)
        : base(id)
    {
        WorkItemId = workItemId;
        UploadedBy = uploadedBy;

        OriginalFileName = originalFileName;
        StoredFileName = storedFileName;

        ContentType = contentType;
        Extension = extension;

        SizeInBytes = sizeInBytes;
        StoragePath = storagePath;

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new AttachmentUploadedDomainEvent(Id));
    }

    private Attachment()
        : base(AttachmentId.Empty())
    {
    }

    public Guid WorkItemId { get; private set; }

    public Guid UploadedBy { get; private set; }

    public string OriginalFileName { get; private set; } = string.Empty;

    public string StoredFileName { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public string Extension { get; private set; } = string.Empty;

    public long SizeInBytes { get; private set; }

    public string StoragePath { get; private set; } = string.Empty;

    public bool IsDeleted { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }
    public Guid? DeletedBy { get; private set; }

    public DateTime? DeletedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }
    public static Attachment Create(
        Guid workItemId,
        Guid uploadedBy,
        string originalFileName,
        string storedFileName,
        string contentType,
        string extension,
        long sizeInBytes,
        string storagePath)
    {
        return new Attachment(
            AttachmentId.New(),
            workItemId,
            uploadedBy,
            originalFileName.Trim(),
            storedFileName.Trim(),
            contentType.Trim(),
            extension.Trim().ToLowerInvariant(),
            sizeInBytes,
            storagePath.Trim());
    }

    public void Delete(Guid userId)
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        DeletedBy = userId;
        DeletedOnUtc = DateTime.UtcNow;
        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new AttachmentDeletedDomainEvent(Id));
    }

    public void Restore()
    {
        if (!IsDeleted)
            return;

        IsDeleted = false;
        DeletedBy = null;
        DeletedOnUtc = null;
        UpdatedOnUtc = DateTime.UtcNow;
    }
}
