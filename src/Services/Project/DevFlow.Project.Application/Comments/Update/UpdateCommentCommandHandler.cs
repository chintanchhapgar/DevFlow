using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Comments.Errors;
using DevFlow.Project.Domain.Comments.Repositories;
using DevFlow.Project.Domain.Comments.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.Update;

internal sealed class UpdateCommentCommandHandler
    : IRequestHandler<
        UpdateCommentCommand,
        Result<UpdateCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommentCommandHandler(
        ICommentRepository commentRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateCommentResponse>> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(
            new CommentId(request.CommentId),
            cancellationToken);

        if (comment is null)
        {
            return Result.Failure<UpdateCommentResponse>(
                CommentErrors.NotFound);
        }

        if (comment.IsDeleted)
        {
            return Result.Failure<UpdateCommentResponse>(
                CommentErrors.AlreadyDeleted);
        }

        if (comment.AuthorId != _currentUser.UserId)
        {
            return Result.Failure<UpdateCommentResponse>(
                CommentErrors.Forbidden);
        }

        comment.Update(request.Content);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateCommentResponse(
                comment.Id.Value,
                comment.Content,
                comment.UpdatedOnUtc),
            "Comment updated successfully.");
    }
}
