using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.Update;

public sealed record UpdateCommentCommand(
    Guid CommentId,
    string Content)
    : IRequest<Result<UpdateCommentResponse>>;
