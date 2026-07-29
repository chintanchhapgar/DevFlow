using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Backlog.RemoveFromSprint;

internal sealed class RemoveWorkItemFromSprintCommandHandler
    : IRequestHandler<
        RemoveWorkItemFromSprintCommand,
        Result<RemoveWorkItemFromSprintResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveWorkItemFromSprintCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RemoveWorkItemFromSprintResponse>> Handle(
        RemoveWorkItemFromSprintCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<RemoveWorkItemFromSprintResponse>(
                WorkItemErrors.NotFound);
        }

        workItem.RemoveFromSprint();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new RemoveWorkItemFromSprintResponse(
                workItem.Id.Value),
            "Work item moved to backlog successfully.");
    }
}
