using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.SharedKernel.Pagination;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.GetAll;

internal sealed class GetAllSprintsQueryHandler
    : IRequestHandler<
        GetAllSprintsQuery,
        Result<PagedList<SprintListItemResponse>>>
{
    private readonly ISprintRepository _repository;

    public GetAllSprintsQueryHandler(
        ISprintRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedList<SprintListItemResponse>>> Handle(
        GetAllSprintsQuery request,
        CancellationToken cancellationToken)
    {
        var pagedSprints =
            await _repository.GetPagedAsync(
                request.ProjectId,
                request.Pagination,
                request.Search,
                cancellationToken);

        var response =
            pagedSprints.Map(x =>
                new SprintListItemResponse(
                    x.Id.Value,
                    x.Name,
                    x.Goal,
                    x.Status,
                    x.StartDate,
                    x.EndDate));

        return Result.Success(
            response,
            "Sprints retrieved successfully.");
    }
}
