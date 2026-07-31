using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.GetSprintBoard;

internal sealed class GetSprintBoardQueryHandler
    : IRequestHandler<
        GetSprintBoardQuery,
        Result<GetSprintBoardResponse>>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IWorkItemRepository _workItemRepository;

    public GetSprintBoardQueryHandler(
        ISprintRepository sprintRepository,
        IWorkItemRepository workItemRepository)
    {
        _sprintRepository = sprintRepository;
        _workItemRepository = workItemRepository;
    }

    public async Task<Result<GetSprintBoardResponse>> Handle(
        GetSprintBoardQuery request,
        CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<GetSprintBoardResponse>(
                AppError.NotFound(
                    "Sprint.NotFound",
                    "Sprint was not found."));
        }

        var workItems = await _workItemRepository.GetBySprintAsync(
            request.SprintId,
            cancellationToken);

        var groupedItems = workItems
            .GroupBy(x => x.Status)
            .ToDictionary(g => g.Key);

        var columns = Enum
            .GetValues<WorkItemStatus>()
            .Select(status =>
            {
                groupedItems.TryGetValue(status, out var items);

                return new SprintBoardColumnResponse(
                    status,
                    (items ?? Enumerable.Empty<WorkItemAggregate>())
                        .OrderBy(x => x.Priority)
                        .ThenBy(x => x.CreatedOnUtc)
                        .Select(x => new SprintBoardWorkItemResponse(
                            x.Id.Value,
                            x.Key,
                            x.Title,
                            x.Type,
                            x.Priority,
                            x.Status,
                            x.AssigneeId,
                            x.ChildCount))
                        .ToList());
            })
            .ToList();

        return Result.Success(
            new GetSprintBoardResponse(
                sprint.Id.Value,
                sprint.Name,
                columns));
    }
}
