using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.ChangePriority;

internal sealed class ChangeWorkItemPriorityCommandHandler
    : IRequestHandler<
        ChangeWorkItemPriorityCommand,
        Result<ChangeWorkItemPriorityResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public ChangeWorkItemPriorityCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<ChangeWorkItemPriorityResponse>> Handle(
        ChangeWorkItemPriorityCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<ChangeWorkItemPriorityResponse>(
                WorkItemErrors.NotFound);
        }

        if (workItem.ReporterId != _currentUser.UserId &&
            workItem.AssigneeId != _currentUser.UserId)
        {
            return Result.Failure<ChangeWorkItemPriorityResponse>(
                WorkItemErrors.Forbidden);
        }

        workItem.ChangePriority(request.Priority);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new ChangeWorkItemPriorityResponse(
                workItem.Id.Value,
                workItem.Priority),
            "Work item priority updated successfully.");
    }
}
