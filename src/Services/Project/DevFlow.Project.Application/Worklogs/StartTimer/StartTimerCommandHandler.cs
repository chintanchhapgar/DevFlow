using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.Project.Domain.Worklogs.Entities;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.StartTimer;

internal sealed class StartTimerCommandHandler
    : IRequestHandler<
        StartTimerCommand,
        Result<StartTimerResponse>>
{
    private readonly IWorklogRepository _repository;
    private readonly IWorkItemRepository _workItems;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public StartTimerCommandHandler(
        IWorklogRepository repository,
        IWorkItemRepository workItems,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _workItems = workItems;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<StartTimerResponse>> Handle(
        StartTimerCommand request,
        CancellationToken cancellationToken)
    {
        var workItem =
            await _workItems.GetByIdAsync(
                new WorkItemId(request.WorkItemId),
                cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<StartTimerResponse>(
                WorkItemErrors.NotFound);
        }

        var worklog = WorklogAggregate.Start(
            request.WorkItemId,
            _currentUser.UserId,
            request.Description);

        await _repository.AddAsync(
            worklog,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new StartTimerResponse(
                worklog.Id.Value,
                worklog.WorkItemId,
                worklog.StartedAtUtc),
            "Timer started successfully.");
    }
}
