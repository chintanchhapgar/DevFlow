using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Sprints.GetAll;

internal sealed class GetAllSprintsQueryHandler
    : IRequestHandler<
        GetAllSprintsQuery,
        Result<GetAllSprintsResponse>>
{
    private readonly ISprintRepository _repository;

    public GetAllSprintsQueryHandler(
        ISprintRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetAllSprintsResponse>> Handle(
        GetAllSprintsQuery request,
        CancellationToken cancellationToken)
    {
        var (items, totalCount) =
            await _repository.GetPagedAsync(
                request.ProjectId,
                request.Page,
                request.PageSize,
                request.Search,
                cancellationToken);

        var response = new GetAllSprintsResponse(
            items.Select(x =>
                new SprintItemResponse(
                    x.Id.Value,
                    x.Name,
                    x.Goal,
                    x.Status,
                    x.StartDate,
                    x.EndDate))
            .ToList(),
            totalCount,
            request.Page,
            request.PageSize);

        return Result.Success(response);
    }
}
