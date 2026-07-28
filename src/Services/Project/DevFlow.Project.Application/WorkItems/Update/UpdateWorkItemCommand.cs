using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Update;

public sealed record UpdateWorkItemCommand(
    Guid WorkItemId,
    string Title,
    string? Description,
    DateTime? DueDate,
    decimal? EstimateHours)
    : IRequest<Result<UpdateWorkItemResponse>>;
