using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.RemoveFromWorkItem;

public sealed record RemoveLabelFromWorkItemCommand(
    Guid WorkItemId,
    Guid LabelId)
    : IRequest<Result<RemoveLabelFromWorkItemResponse>>;
