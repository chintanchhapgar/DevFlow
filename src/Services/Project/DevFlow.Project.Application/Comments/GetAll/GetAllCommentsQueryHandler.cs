using DevFlow.Project.Domain.Comments.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.GetAll;

internal sealed class GetAllCommentsQueryHandler
    : IRequestHandler<
        GetAllCommentsQuery,
        Result<IReadOnlyList<GetAllCommentsResponse>>>
{
    private readonly ICommentRepository _commentRepository;

    public GetAllCommentsQueryHandler(
        ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllCommentsResponse>>> Handle(
        GetAllCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetByWorkItemAsync(
            request.WorkItemId,
            cancellationToken);

        var response = comments
            .Select(comment =>
                new GetAllCommentsResponse(
                    comment.Id.Value,
                    comment.AuthorId,
                    comment.Content,
                    comment.CreatedOnUtc,
                    comment.UpdatedOnUtc))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyList<GetAllCommentsResponse>>(
            response,
            "Comments retrieved successfully.");
    }
}
