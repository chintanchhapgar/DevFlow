using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Dashboard.GetDashboard;

internal sealed class GetDashboardQueryHandler
    : IRequestHandler<GetDashboardQuery, Result<GetDashboardResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IWorkItemRepository _workItemRepository;

    public GetDashboardQueryHandler(
        IProjectRepository projectRepository,
        ISprintRepository sprintRepository,
        IWorkItemRepository workItemRepository)
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _workItemRepository = workItemRepository;
    }

    public async Task<Result<GetDashboardResponse>> Handle(
        GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<GetDashboardResponse>(
                AppError.NotFound(
                    "Project.NotFound",
                    "Project was not found."));
        }

        var activeSprint = await _sprintRepository.GetActiveSprintAsync(
            request.ProjectId,
            cancellationToken);

        IReadOnlyList<Domain.WorkItems.Entities.WorkItemAggregate> workItems;

        if (activeSprint is null)
        {
            workItems = await _workItemRepository.GetBacklogAsync(
                request.ProjectId,
                cancellationToken);
        }
        else
        {
            workItems = await _workItemRepository.GetBySprintAsync(
                activeSprint.Id.Value,
                cancellationToken);
        }

        var total = workItems.Count;

        var todo = workItems.Count(x => x.Status == WorkItemStatus.Todo);
        var inProgress = workItems.Count(x => x.Status == WorkItemStatus.InProgress);
        var review = workItems.Count(x => x.Status == WorkItemStatus.InReview);
        var done = workItems.Count(x => x.Status == WorkItemStatus.Done);

        DashboardSprintResponse? sprint = null;

        if (activeSprint is not null)
        {
            var remainingDays = Math.Max(
                0,
                activeSprint.EndDate.DayNumber -
                DateOnly.FromDateTime(DateTime.UtcNow).DayNumber);

            var completion =
                total == 0
                    ? 0
                    : Math.Round(done * 100d / total, 2);

            sprint = new DashboardSprintResponse(
                activeSprint.Id.Value,
                activeSprint.Name,
                activeSprint.StartDate,
                activeSprint.EndDate,
                remainingDays,
                completion);
        }

        var assignedToMe = workItems
            .Where(x => x.AssigneeId.HasValue)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.CreatedOnUtc)
            .Take(10)
            .Select(x => new DashboardAssignedWorkItemResponse(
                x.Id.Value,
                x.Key,
                x.Title,
                x.Status,
                x.Priority,
                x.DueDate))
            .ToList();

        var response = new GetDashboardResponse(
            new DashboardProjectResponse(
                project.Id.Value,
                project.Key,
                project.Name,
                project.Description,
                project.Members.Count),
            new DashboardMetricsResponse(
                total,
                todo,
                inProgress,
                review,
                done),
            sprint,
            assignedToMe,
            new List<DashboardRecentActivityResponse>());

        return Result.Success(response);
    }
}
