using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Delete;

public sealed record DeleteWorkItemCommand(
    Guid WorkItemId)
    : IRequest<Result<DeleteWorkItemResponse>>;
