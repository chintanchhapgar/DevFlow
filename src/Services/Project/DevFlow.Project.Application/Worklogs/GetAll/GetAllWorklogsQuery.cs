using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.GetAll;

public sealed record GetAllWorklogsQuery(
    Guid WorkItemId)
    : IRequest<Result<IReadOnlyList<GetAllWorklogsResponse>>>;
