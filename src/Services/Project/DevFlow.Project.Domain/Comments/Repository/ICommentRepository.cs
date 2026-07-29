
using DevFlow.Project.Domain.Comments.Entities;
using DevFlow.Project.Domain.Comments.ValueObjects;

namespace DevFlow.Project.Domain.Comments.Repositories;

public interface ICommentRepository
{
    Task AddAsync(
        Comment comment,
        CancellationToken cancellationToken = default);

    Task<Comment?> GetByIdAsync(
        CommentId id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Comment>> GetByWorkItemAsync(
        Guid workItemId,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Comment comment,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Comment comment,
        CancellationToken cancellationToken = default);
}
