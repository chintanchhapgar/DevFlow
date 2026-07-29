using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Backlog.GetBacklog;

internal sealed class GetBacklogQueryHandler
    : IRequestHandler<
        GetBacklogQuery,
        Result<GetBacklogResponse>>
{
    private readonly IWorkItemRepository _repository;

    public GetBacklogQueryHandler(
        IWorkItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetBacklogResponse>> Handle(
        GetBacklogQuery request,
        CancellationToken cancellationToken)
    {
        var items = await _repository.GetBacklogAsync(
            request.ProjectId,
            cancellationToken);

        return Result.Success(
            new GetBacklogResponse(
                items.Select(x =>
                    new BacklogWorkItemResponse(
                        x.Id.Value,
                        x.Key,
                        x.Title,
                        x.Type,
                        x.Priority,
                        x.Status,
                        x.AssigneeId,
                        x.EstimateHours))
                .ToList()));
    }
}
