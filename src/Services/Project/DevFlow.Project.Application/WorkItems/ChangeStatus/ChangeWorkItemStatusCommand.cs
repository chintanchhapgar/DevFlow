using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.ChangeStatus;

public sealed record ChangeWorkItemStatusCommand(
    Guid WorkItemId,
    WorkItemStatus Status)
    : IRequest<Result<ChangeWorkItemStatusResponse>>;
