using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Epics.Errors;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.Project.Domain.Epics.ValueObjects;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Epics.RemoveWorkItem;

internal sealed class RemoveWorkItemCommandHandler
    : IRequestHandler<
        RemoveWorkItemCommand,
        Result<RemoveWorkItemResponse>>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveWorkItemCommandHandler(
        IEpicRepository epicRepository,
        IWorkItemRepository workItemRepository,
        IUnitOfWork unitOfWork)
    {
        _epicRepository = epicRepository;
        _workItemRepository = workItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RemoveWorkItemResponse>> Handle(
        RemoveWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var epic = await _epicRepository.GetByIdAsync(
            new EpicId(request.EpicId),
            cancellationToken);

        if (epic is null)
        {
            return Result.Failure<RemoveWorkItemResponse>(
                EpicErrors.NotFound);
        }

        var workItem = await _workItemRepository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<RemoveWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        workItem.RemoveEpic();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new RemoveWorkItemResponse(
                epic.Id.Value,
                workItem.Id.Value),
            "Work item removed from epic successfully.");
    }
}
