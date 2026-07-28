using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.GetAll;

internal sealed class GetAllWorkItemsQueryHandler
    : IRequestHandler<
        GetAllWorkItemsQuery,
        Result<GetAllWorkItemsResponse>>
{
    private readonly IWorkItemRepository _repository;

    public GetAllWorkItemsQueryHandler(
        IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetAllWorkItemsResponse>> Handle(
        GetAllWorkItemsQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) =
            await _repository.GetPagedAsync(
                request.ProjectId,
                request.Page,
                request.PageSize,
                request.Search,
                request.Status,
                request.Type,
                request.Priority,
                request.AssigneeId,
                cancellationToken);

        return Result.Success(
            new GetAllWorkItemsResponse(
                items.Select(x =>
                    new GetAllWorkItemItem(
                        x.Id.Value,
                        x.Key,
                        x.Title,
                        x.Type,
                        x.Status,
                        x.Priority,
                        x.AssigneeId,
                        x.DueDate))
                .ToList(),
                totalCount,
                request.Page,
                request.PageSize));
    }
}
