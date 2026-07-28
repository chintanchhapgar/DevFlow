using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.GetById;

internal sealed class GetWorkItemByIdQueryHandler
    : IRequestHandler<GetWorkItemByIdQuery, Result<GetWorkItemByIdResponse>>
{
    private readonly IWorkItemRepository _repository;

    public GetWorkItemByIdQueryHandler(
        IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetWorkItemByIdResponse>> Handle(
        GetWorkItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        WorkItemAggregate? workItem =
            await _repository.GetByIdAsync(
                new(request.WorkItemId),
                cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<GetWorkItemByIdResponse>(
                WorkItemErrors.NotFound);
        }

        return Result.Success(
            new GetWorkItemByIdResponse(
                workItem.Id.Value,
                workItem.ProjectId,
                workItem.Key,
                workItem.Title,
                workItem.Description,
                workItem.Type,
                workItem.Status,
                workItem.Priority,
                workItem.AssigneeId,
                workItem.ReporterId,
                workItem.EpicId,
                workItem.ParentId,
                workItem.SprintId,
                workItem.EstimateHours,
                workItem.DueDate,
                workItem.CreatedOnUtc,
                workItem.UpdatedOnUtc));
    }
}
