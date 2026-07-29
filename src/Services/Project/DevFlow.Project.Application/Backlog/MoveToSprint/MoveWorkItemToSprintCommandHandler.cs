using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Sprints.Errors;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Backlog.MoveToSprint;

internal sealed class MoveWorkItemToSprintCommandHandler
    : IRequestHandler<
        MoveWorkItemToSprintCommand,
        Result<MoveWorkItemToSprintResponse>>
{
    private readonly IWorkItemRepository _workItemRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MoveWorkItemToSprintCommandHandler(
        IWorkItemRepository workItemRepository,
        ISprintRepository sprintRepository,
        IUnitOfWork unitOfWork)
    {
        _workItemRepository = workItemRepository;
        _sprintRepository = sprintRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MoveWorkItemToSprintResponse>> Handle(
        MoveWorkItemToSprintCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _workItemRepository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<MoveWorkItemToSprintResponse>(
                WorkItemErrors.NotFound);
        }

        var sprint = await _sprintRepository.GetByIdAsync(
            new SprintId(request.SprintId),
            cancellationToken);

        if (sprint is null)
        {
            return Result.Failure<MoveWorkItemToSprintResponse>(
                SprintErrors.NotFound);
        }

        workItem.MoveToSprint(request.SprintId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new MoveWorkItemToSprintResponse(
                workItem.Id.Value,
                sprint.Id.Value),
            "Work item moved to sprint successfully.");
    }
}
