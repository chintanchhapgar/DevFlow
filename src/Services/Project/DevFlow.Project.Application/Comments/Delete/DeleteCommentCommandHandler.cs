using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Comments.Errors;
using DevFlow.Project.Domain.Comments.Repositories;
using DevFlow.Project.Domain.Comments.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.Delete;

internal sealed class DeleteCommentCommandHandler
    : IRequestHandler<
        DeleteCommentCommand,
        Result<DeleteCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DeleteCommentResponse>> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(
            new CommentId(request.CommentId),
            cancellationToken);

        if (comment is null)
        {
            return Result.Failure<DeleteCommentResponse>(
                CommentErrors.NotFound);
        }

        if (comment.IsDeleted)
        {
            return Result.Failure<DeleteCommentResponse>(
                CommentErrors.AlreadyDeleted);
        }

        if (comment.AuthorId != _currentUser.UserId)
        {
            return Result.Failure<DeleteCommentResponse>(
                CommentErrors.Forbidden);
        }

        comment.Delete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeleteCommentResponse(
                comment.Id.Value),
            "Comment deleted successfully.");
    }
}
