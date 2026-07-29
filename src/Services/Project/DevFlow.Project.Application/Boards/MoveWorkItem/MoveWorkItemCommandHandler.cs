using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Boards.MoveWorkItem;

internal sealed class MoveWorkItemCommandHandler
    : IRequestHandler<
        MoveWorkItemCommand,
        Result<MoveWorkItemResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public MoveWorkItemCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MoveWorkItemResponse>> Handle(
        MoveWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<MoveWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        workItem.MoveToStatus(request.Status);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new MoveWorkItemResponse(
                workItem.Id.Value,
                workItem.Status),
            "Work item moved successfully.");
    }
}
