using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.GetAll;

public sealed record GetAllWorkItemsQuery(
    Guid ProjectId,
    int Page,
    int PageSize,
    string? Search,
    WorkItemStatus? Status,
    WorkItemType? Type,
    WorkItemPriority? Priority,
    Guid? AssigneeId)
    : IRequest<Result<GetAllWorkItemsResponse>>;
