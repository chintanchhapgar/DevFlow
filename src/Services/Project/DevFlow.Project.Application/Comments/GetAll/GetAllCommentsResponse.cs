namespace DevFlow.Project.Application.Comments.GetAll;

public sealed record GetAllCommentsResponse(
    Guid CommentId,
    Guid AuthorId,
    string Content,
    DateTime CreatedOnUtc,
    DateTime? UpdatedOnUtc);
