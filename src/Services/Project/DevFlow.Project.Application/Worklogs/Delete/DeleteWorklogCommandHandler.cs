using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Worklogs.Errors;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.Project.Domain.Worklogs.ValueObjects;
using DevFlow.SharedKernel.Results;
using DevFlow.SharedKernel.Common;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Delete;

internal sealed class DeleteWorklogCommandHandler
    : IRequestHandler<
        DeleteWorklogCommand,
        Result<DeleteWorklogResponse>>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeleteWorklogCommandHandler(
        IWorklogRepository worklogRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _worklogRepository = worklogRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<DeleteWorklogResponse>> Handle(
        DeleteWorklogCommand request,
        CancellationToken cancellationToken)
    {
        var worklog =
            await _worklogRepository.GetByIdAsync(
                new WorklogId(request.WorklogId),
                cancellationToken);

        if (worklog is null)
        {
            return Result.Failure<DeleteWorklogResponse>(
                WorklogErrors.NotFound);
        }

        if (worklog.IsDeleted)
        {
            return Result.Failure<DeleteWorklogResponse>(
                WorklogErrors.AlreadyDeleted);
        }

        if (worklog.UserId != _currentUser.UserId)
        {
            return Result.Failure<DeleteWorklogResponse>(
                WorklogErrors.Forbidden);
        }

        worklog.Delete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeleteWorklogResponse(
                worklog.Id.Value,
                worklog.WorkItemId,
                DateTime.UtcNow),
            "Worklog deleted successfully.");
    }
}
