using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.Create;

public sealed record CreateCommentCommand(
    Guid WorkItemId,
    string Content)
    : IRequest<Result<CreateCommentResponse>>;
