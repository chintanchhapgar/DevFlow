using DevFlow.Project.Domain.Comments.Entities;
using DevFlow.Project.Domain.Comments.Repositories;
using DevFlow.Project.Domain.Comments.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Project.Infrastructure.Persistence.Repositories;

internal sealed class CommentRepository
    : ICommentRepository
{
    private readonly ProjectDbContext _context;

    public CommentRepository(
        ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Comment comment,
        CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(
            comment,
            cancellationToken);
    }

    public async Task<Comment?> GetByIdAsync(
        CommentId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Comment>> GetByWorkItemAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(x =>
                x.WorkItemId == workItemId &&
                !x.IsDeleted)
            .OrderBy(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public Task UpdateAsync(
        Comment comment,
        CancellationToken cancellationToken = default)
    {
        _context.Comments.Update(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        Comment comment,
        CancellationToken cancellationToken = default)
    {
        _context.Comments.Remove(comment);

        return Task.CompletedTask;
    }
}
