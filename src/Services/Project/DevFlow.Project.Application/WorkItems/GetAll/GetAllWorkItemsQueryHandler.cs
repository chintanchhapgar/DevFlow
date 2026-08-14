using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Pagination;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.GetAll;

internal sealed class GetAllWorkItemsQueryHandler
    : IRequestHandler<
        GetAllWorkItemsQuery,
        Result<PagedList<WorkItemListItemResponse>>>
{
    private readonly IWorkItemRepository _repository;

    public GetAllWorkItemsQueryHandler(
        IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedList<WorkItemListItemResponse>>> Handle(
        GetAllWorkItemsQuery request,
        CancellationToken cancellationToken)
    {
        var pagedWorkItems =
            await _repository.GetPagedAsync(
                request.ProjectId,
                request.Pagination,
                request.Search,
                request.Status,
                request.Type,
                request.Priority,
                request.AssigneeId,
                cancellationToken);

        var response =
            pagedWorkItems.Map(x =>
                new WorkItemListItemResponse(
                    x.Id.Value,
                    x.Key,
                    x.Title,
                    x.Description,        // ✅ Added
                    x.Type,
                    x.Status,
                    x.Priority,
                    x.AssigneeId,
                    x.SprintId,           // ✅ Added
                    x.EstimateHours,      // ✅ Added
                    x.DueDate));

        return Result.Success(
            response,
            "Work items retrieved successfully.");
    }
}
