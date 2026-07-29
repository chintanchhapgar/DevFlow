namespace DevFlow.Project.Application.Comments.Update;

public sealed record UpdateCommentResponse(
    Guid CommentId,
    string Content,
    DateTime? UpdatedOnUtc);
