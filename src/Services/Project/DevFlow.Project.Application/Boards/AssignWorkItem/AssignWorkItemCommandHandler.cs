using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.AssignWorkItem;

internal sealed class AssignWorkItemCommandHandler
    : IRequestHandler<
        AssignWorkItemCommand,
        Result<AssignWorkItemResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignWorkItemCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssignWorkItemResponse>> Handle(
        AssignWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<AssignWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        workItem.Assign(request.AssigneeId);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new AssignWorkItemResponse(
                workItem.Id.Value,
                request.AssigneeId),
            "Work item assigned successfully.");
    }
}
