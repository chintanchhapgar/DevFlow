using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.GetBoard;

internal sealed class GetBoardQueryHandler
    : IRequestHandler<
        GetBoardQuery,
        Result<GetBoardResponse>>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IWorkItemRepository _workItemRepository;

    public GetBoardQueryHandler(
        ISprintRepository sprintRepository,
        IWorkItemRepository workItemRepository)
    {
        _sprintRepository = sprintRepository;
        _workItemRepository = workItemRepository;
    }

    public async Task<Result<GetBoardResponse>> Handle(
        GetBoardQuery request,
        CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetActiveSprintAsync(
            request.ProjectId,
            cancellationToken);

        if (sprint is null)
        {
            return Result.Success(
                new GetBoardResponse(
                    null,
                    new List<BoardColumnResponse>()));
        }

        var workItems = await _workItemRepository.GetBySprintAsync(
            sprint.Id.Value,
            cancellationToken);

        var columns = Enum
            .GetValues<WorkItemStatus>()
            .Select(status =>
                new BoardColumnResponse(
                    status,
                    workItems
                        .Where(x => x.Status == status)
                        .Select(x =>
                            new BoardWorkItemResponse(
                                x.Id.Value,
                                x.Key,
                                x.Title,
                                x.Type,
                                x.Priority,
                                x.AssigneeId))
                        .ToList()))
            .ToList();

        return Result.Success(
            new GetBoardResponse(
                new ActiveSprintResponse(
                    sprint.Id.Value,
                    sprint.Name),
                columns));
    }
}
