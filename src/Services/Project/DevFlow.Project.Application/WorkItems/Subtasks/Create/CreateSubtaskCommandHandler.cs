using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Entities;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Subtasks.Create;

internal sealed class CreateSubtaskCommandHandler
    : IRequestHandler<
        CreateSubtaskCommand,
        Result<CreateSubtaskResponse>>
{
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateSubtaskCommandHandler(
        IWorkItemRepository workItemRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _workItemRepository = workItemRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<CreateSubtaskResponse>> Handle(
        CreateSubtaskCommand request,
        CancellationToken cancellationToken)
    {
        var parent = await _workItemRepository.GetByIdAsync(
            new WorkItemId(request.ParentId),
            cancellationToken);

        if (parent is null)
        {
            return Result.Failure<CreateSubtaskResponse>(
                WorkItemErrors.NotFound);
        }

        var sequence = parent.GetNextChildSequence();

        var key = $"{parent.Key}-{sequence}";

        var subtask = WorkItemAggregate.Create(
             parent.ProjectId,
             key,
             request.Title,
             request.Description,
             WorkItemType.SubTask,
             request.Priority,
             _currentUser.UserId,
             null,
             null,
             null);

        subtask.SetParent(parent.Id.Value);

        subtask.ChangePriority(request.Priority);


        await _workItemRepository.AddAsync(
            subtask,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateSubtaskResponse(
                subtask.Id.Value,
                parent.Id.Value,
                subtask.Key),
            "Subtask created successfully.");
    }
}
