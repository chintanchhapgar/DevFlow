namespace DevFlow.Project.Application.Comments.Create;

public sealed record CreateCommentResponse(
    Guid CommentId,
    Guid WorkItemId,
    Guid AuthorId,
    string Content,
    DateTime CreatedOnUtc);
