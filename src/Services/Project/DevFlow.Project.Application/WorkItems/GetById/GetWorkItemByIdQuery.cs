using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.GetById;

public sealed record GetWorkItemByIdQuery(
    Guid WorkItemId)
    : IRequest<Result<GetWorkItemByIdResponse>>;
