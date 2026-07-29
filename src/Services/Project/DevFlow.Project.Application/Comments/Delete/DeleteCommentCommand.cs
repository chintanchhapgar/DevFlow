using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.Delete;

public sealed record DeleteCommentCommand(
    Guid CommentId)
    : IRequest<Result<DeleteCommentResponse>>;
