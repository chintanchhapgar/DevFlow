using DevFlow.Project.Domain.Attachments.Entities;
using DevFlow.Project.Domain.Attachments.ValueObjects;

namespace DevFlow.Project.Domain.Attachments.Repositories;

public interface IAttachmentRepository
{
    Task AddAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default);

    Task<Attachment?> GetByIdAsync(
        AttachmentId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Attachment>> GetByWorkItemAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default);
}
