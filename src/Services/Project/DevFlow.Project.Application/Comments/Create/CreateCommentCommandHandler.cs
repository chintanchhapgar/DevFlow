using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Comments.Entities;
using DevFlow.Project.Domain.Comments.Repositories;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.Create;

internal sealed class CreateCommentCommandHandler
    : IRequestHandler<
        CreateCommentCommand,
        Result<CreateCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        IWorkItemRepository workItemRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _workItemRepository = workItemRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateCommentResponse>> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _workItemRepository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<CreateCommentResponse>(
                WorkItemErrors.NotFound);
        }

        var comment = Comment.Create(
            request.WorkItemId,
            _currentUser.UserId,
            request.Content);

        await _commentRepository.AddAsync(
            comment,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateCommentResponse(
                comment.Id.Value,
                comment.WorkItemId,
                comment.AuthorId,
                comment.Content,
                comment.CreatedOnUtc),
            "Comment created successfully.");
    }
}
