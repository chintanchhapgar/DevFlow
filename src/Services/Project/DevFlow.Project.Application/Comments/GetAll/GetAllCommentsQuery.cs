using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Comments.GetAll;

public sealed record GetAllCommentsQuery(
    Guid WorkItemId)
    : IRequest<Result<IReadOnlyList<GetAllCommentsResponse>>>;
