using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Update;

internal sealed class UpdateWorkItemCommandHandler
    : IRequestHandler<
        UpdateWorkItemCommand,
        Result<UpdateWorkItemResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateWorkItemCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<UpdateWorkItemResponse>> Handle(
        UpdateWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<UpdateWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        // Reporter or assignee can edit
        if (workItem.ReporterId != _currentUser.UserId &&
            workItem.AssigneeId != _currentUser.UserId)
        {
            return Result.Failure<UpdateWorkItemResponse>(
                WorkItemErrors.Forbidden);
        }

        workItem.Update(
            request.Title,
            request.Description,
            NormalizeUtc(request.DueDate),
            request.EstimateHours);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new UpdateWorkItemResponse(
                workItem.Id.Value,
                workItem.Title),
            "Work item updated successfully.");
    }

    private static DateTime? NormalizeUtc(DateTime? value)
    {
        if (value is null)
        {
            return null;
        }

        return value.Value.Kind switch
        {
            DateTimeKind.Utc => value.Value,
            DateTimeKind.Local => value.Value.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(
                value.Value,
                DateTimeKind.Utc),
            _ => value.Value
        };
    }
}
