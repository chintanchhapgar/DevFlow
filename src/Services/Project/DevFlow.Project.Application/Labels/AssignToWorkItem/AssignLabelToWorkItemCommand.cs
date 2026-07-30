using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.AssignToWorkItem;

public sealed record AssignLabelToWorkItemCommand(
    Guid WorkItemId,
    Guid LabelId)
    : IRequest<Result<AssignLabelToWorkItemResponse>>;
