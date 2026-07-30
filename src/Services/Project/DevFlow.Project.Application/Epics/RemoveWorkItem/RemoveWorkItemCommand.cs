using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.RemoveWorkItem;

public sealed record RemoveWorkItemCommand(
    Guid EpicId,
    Guid WorkItemId)
    : IRequest<Result<RemoveWorkItemResponse>>;
