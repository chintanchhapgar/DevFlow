using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.ChangeStatus;

internal sealed class ChangeWorkItemStatusCommandHandler
    : IRequestHandler<
        ChangeWorkItemStatusCommand,
        Result<ChangeWorkItemStatusResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public ChangeWorkItemStatusCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<ChangeWorkItemStatusResponse>> Handle(
        ChangeWorkItemStatusCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<ChangeWorkItemStatusResponse>(
                WorkItemErrors.NotFound);
        }

        if (workItem.ReporterId != _currentUser.UserId &&
            workItem.AssigneeId != _currentUser.UserId)
        {
            return Result.Failure<ChangeWorkItemStatusResponse>(
                WorkItemErrors.Forbidden);
        }

        workItem.ChangeStatus(request.Status);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new ChangeWorkItemStatusResponse(
                workItem.Id.Value,
                workItem.Status),
            "Work item status updated successfully.");
    }
}
