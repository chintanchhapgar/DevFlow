using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.WorkItems.Errors;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.WorkItems.ValueObjects;
using DevFlow.Project.Domain.Worklogs.Entities;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Create;

internal sealed class CreateWorklogCommandHandler
    : IRequestHandler<
        CreateWorklogCommand,
        Result<CreateWorklogResponse>>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly IWorkItemRepository _workItemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateWorklogCommandHandler(
        IWorklogRepository worklogRepository,
        IWorkItemRepository workItemRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _worklogRepository = worklogRepository;
        _workItemRepository = workItemRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<CreateWorklogResponse>> Handle(
        CreateWorklogCommand request,
        CancellationToken cancellationToken)
    {
        var workItem =
            await _workItemRepository.GetByIdAsync(
                new WorkItemId(request.WorkItemId),
                cancellationToken);

        if (workItem is null)
        {
            return Result.Failure<CreateWorklogResponse>(
                WorkItemErrors.NotFound);
        }

        var worklog = WorklogAggregate.Create(
            request.WorkItemId,
            _currentUser.UserId,
            request.Description,
            request.StartedAtUtc,
            request.EndedAtUtc);

        await _worklogRepository.AddAsync(
            worklog,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new CreateWorklogResponse(
                worklog.Id.Value,
                worklog.WorkItemId,
                worklog.UserId,
                worklog.Description,
                worklog.StartedAtUtc,
                worklog.IsRunning),
            "Worklog created successfully.");
    }
}
