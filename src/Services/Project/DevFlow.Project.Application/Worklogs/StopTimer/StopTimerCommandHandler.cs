using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Worklogs.Errors;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.StopTimer;

internal sealed class StopTimerCommandHandler
    : IRequestHandler<
        StopTimerCommand,
        Result<StopTimerResponse>>
{
    private readonly IWorklogRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public StopTimerCommandHandler(
        IWorklogRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<StopTimerResponse>> Handle(
        StopTimerCommand request,
        CancellationToken cancellationToken)
    {
        var worklog =
            await _repository.GetRunningWorklogAsync(
                request.WorkItemId,
                _currentUser.UserId,
                cancellationToken);

        if (worklog is null)
        {
            return Result.Failure<StopTimerResponse>(
                WorklogErrors.NotFound);
        }

        worklog.Stop(DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new StopTimerResponse(
                worklog.Id.Value,
                worklog.WorkItemId,
                worklog.StartedAtUtc,
                worklog.EndedAtUtc!.Value,
                worklog.MinutesSpent),
            "Timer stopped successfully.");
    }
}
