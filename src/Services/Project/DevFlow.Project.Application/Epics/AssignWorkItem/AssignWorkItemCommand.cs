using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.AssignWorkItem;

public sealed record AssignWorkItemCommand(
    Guid EpicId,
    Guid WorkItemId)
    : IRequest<Result<AssignWorkItemResponse>>;
