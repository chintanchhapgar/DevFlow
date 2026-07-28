using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Assign;

internal sealed class AssignWorkItemCommandHandler
    : IRequestHandler<
        AssignWorkItemCommand,
        Result<AssignWorkItemResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AssignWorkItemCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<AssignWorkItemResponse>> Handle(
        AssignWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<AssignWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        if (workItem.ReporterId != _currentUser.UserId)
        {
            return Result.Failure<AssignWorkItemResponse>(
                WorkItemErrors.Forbidden);
        }

        workItem.Assign(request.AssigneeId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new AssignWorkItemResponse(
                workItem.Id.Value,
                request.AssigneeId),
            "Work item assigned successfully.");
    }
}
