using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Labels.Errors;
using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.Project.Domain.Labels.ValueObjects;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.AssignToWorkItem;

internal sealed class AssignLabelToWorkItemCommandHandler
    : IRequestHandler<
        AssignLabelToWorkItemCommand,
        Result<AssignLabelToWorkItemResponse>>
{
    private readonly IWorkItemRepository _workItemRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignLabelToWorkItemCommandHandler(
        IWorkItemRepository workItemRepository,
        ILabelRepository labelRepository,
        IUnitOfWork unitOfWork)
    {
        _workItemRepository = workItemRepository;
        _labelRepository = labelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssignLabelToWorkItemResponse>> Handle(
        AssignLabelToWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var workItem =
            await _workItemRepository.GetByIdAsync(
                new WorkItemId(request.WorkItemId),
                cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<AssignLabelToWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        var label =
            await _labelRepository.GetByIdAsync(
                new LabelId(request.LabelId),
                cancellationToken);

        if (label is null)
        {
            return Result.Failure<AssignLabelToWorkItemResponse>(
                LabelErrors.NotFound);
        }

        workItem.AddLabel(label.Id.Value);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new AssignLabelToWorkItemResponse(
                workItem.Id.Value,
                label.Id.Value),
            "Label assigned successfully.");
    }
}
