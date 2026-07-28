using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.ChangePriority;

public sealed record ChangeWorkItemPriorityCommand(
    Guid WorkItemId,
    WorkItemPriority Priority)
    : IRequest<Result<ChangeWorkItemPriorityResponse>>;
