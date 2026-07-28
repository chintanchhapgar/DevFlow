using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.WorkItems.Delete;

internal sealed class DeleteWorkItemCommandHandler
    : IRequestHandler<
        DeleteWorkItemCommand,
        Result<DeleteWorkItemResponse>>
{
    private readonly IWorkItemRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeleteWorkItemCommandHandler(
        IWorkItemRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<DeleteWorkItemResponse>> Handle(
        DeleteWorkItemCommand request,
        CancellationToken cancellationToken)
    {
        var workItem = await _repository.GetByIdAsync(
            new(request.WorkItemId),
            cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<DeleteWorkItemResponse>(
                WorkItemErrors.NotFound);
        }

        if (workItem.ReporterId != _currentUser.UserId)
        {
            return Result.Failure<DeleteWorkItemResponse>(
                WorkItemErrors.Forbidden);
        }

        workItem.Delete();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(
            new DeleteWorkItemResponse(
                workItem.Id.Value,
                "Deleted"),
            "Work item deleted successfully.");
    }
}
