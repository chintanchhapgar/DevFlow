using DevFlow.Project.Domain.Comments.Events;
using DevFlow.Project.Domain.Comments.ValueObjects;
using DevFlow.SharedKernel.Domain;

namespace DevFlow.Project.Domain.Comments.Entities;

public sealed class Comment
    : AggregateRoot<CommentId>
{
    private Comment(
        CommentId id,
        Guid workItemId,
        Guid authorId,
        string content)
        : base(id)
    {
        WorkItemId = workItemId;
        AuthorId = authorId;
        Content = content.Trim();

        CreatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new CommentCreatedDomainEvent(Id));
    }

    private Comment()
        : base(CommentId.Empty())
    {
    }

    public Guid WorkItemId { get; private set; }

    public Guid AuthorId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public bool IsDeleted { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? UpdatedOnUtc { get; private set; }

    public static Comment Create(
        Guid workItemId,
        Guid authorId,
        string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new Comment(
            CommentId.New(),
            workItemId,
            authorId,
            content);
    }

    public void Update(
        string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        var newContent = content.Trim();

        if (Content == newContent)
            return;

        Content = newContent;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new CommentUpdatedDomainEvent(Id));
    }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;

        UpdatedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(
            new CommentDeletedDomainEvent(Id));
    }

    public void Restore()
    {
        if (!IsDeleted)
            return;

        IsDeleted = false;

        UpdatedOnUtc = DateTime.UtcNow;
    }
}
