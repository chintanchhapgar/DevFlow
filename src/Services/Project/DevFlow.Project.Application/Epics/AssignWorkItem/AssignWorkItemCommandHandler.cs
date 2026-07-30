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

namespace DevFlow.Project.Application.Epics.AssignWorkItem;

internal sealed class AssignWorkItemCommandHandler
    : IRequestHandler<
        AssignWorkItemCommand,
        Result<AssignWorkItemResponse>>
{
    private readonly IEpicRepository _epicRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignWorkItemCommandHandler(
        IEpicRepository epicRepository,
        IWorkItemRepository workItemRepository,
        IUnitOfWork unitOfWork)
    {
        _epicRepository = epicRepository;
        _workItemRepository = workItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssignWorkItemResponse>> Handle(
        AssignWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var epic = await _epicRepository.GetByIdAsync(
            new EpicId(request.EpicId),
            cancellationToken);

        if (epic is null)
        {
            return Result.Failure<AssignWorkItemResponse>(
                EpicErrors.NotFound);
        }

        var workItem = await _workItemRepository.GetByIdAsync(
            new WorkItemId(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<AssignWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        workItem.LinkEpic(epic.Id.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new AssignWorkItemResponse(
                epic.Id.Value,
                workItem.Id.Value),
            "Work item assigned to epic successfully.");
    }
}
