using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Create;

public sealed record CreateWorkItemCommand(
    Guid ProjectId,
    string Title,
    string? Description,
    WorkItemType Type,
    WorkItemPriority Priority,
    Guid? AssigneeId,
    DateTime? DueDate,
    decimal? EstimateHours)
    : IRequest<Result<CreateWorkItemResponse>>;
