using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
//using DevFlow.Project.Domain.Sprints.Repositories;
//using DevFlow.Project.Domain.Sprints.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.MoveToSprint;

internal sealed class MoveWorkItemToSprintCommandHandler
    : IRequestHandler<
        MoveWorkItemToSprintCommand,
        Result<MoveWorkItemToSprintResponse>>
{
    private readonly IWorkItemRepository _workItemRepository;
    //private readonly ISprintRepository _sprintRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public MoveWorkItemToSprintCommandHandler(
        IWorkItemRepository workItemRepository,
        //ISprintRepository sprintRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _workItemRepository = workItemRepository;
        //_sprintRepository = sprintRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<MoveWorkItemToSprintResponse>> Handle(
        MoveWorkItemToSprintCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _workItemRepository.GetByIdAsync(
            new(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<MoveWorkItemToSprintResponse>(
                WorkItemErrors.NotFound);
        }

        if (workItem.ReporterId != _currentUser.UserId &&
            workItem.AssigneeId != _currentUser.UserId)
        {
            return Result.Failure<MoveWorkItemToSprintResponse>(
                WorkItemErrors.Forbidden);
        }

        //var sprint = await _sprintRepository.GetByIdAsync(
        //    new SprintId(request.SprintId),
        //    cancellationToken);

        //if (sprint is null)
        //{
        //    return Result.Failure<MoveWorkItemToSprintResponse>(
        //        SprintErrors.NotFound);
        //}

        workItem.MoveToSprint(request.SprintId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new MoveWorkItemToSprintResponse(
                workItem.Id.Value,
                request.SprintId),
            "Work item moved to sprint successfully.");
    }
}
