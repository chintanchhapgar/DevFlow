using DevFlow.Project.Domain.Attachments.Entities;
using DevFlow.Project.Domain.Attachments.Repositories;
using DevFlow.Project.Domain.Attachments.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class AttachmentRepository
    : IAttachmentRepository
{
    private readonly ProjectDbContext _context;

    public AttachmentRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default)
    {
        await _context.Attachments.AddAsync(
            attachment,
            cancellationToken);
    }

    public async Task<Attachment?> GetByIdAsync(
        AttachmentId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Attachments
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Attachment>> GetByWorkItemAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Attachments
            .Where(x =>
                x.WorkItemId == workItemId &&
                !x.IsDeleted)
            .OrderBy(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default)
    {
        _context.Attachments.Update(attachment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        Attachment attachment,
        CancellationToken cancellationToken = default)
    {
        _context.Attachments.Remove(attachment);

        return Task.CompletedTask;
    }
}
