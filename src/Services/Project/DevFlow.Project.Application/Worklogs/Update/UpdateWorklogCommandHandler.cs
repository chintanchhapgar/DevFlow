using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Worklogs.Errors;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.Project.Domain.Worklogs.ValueObjects;
using DevFlow.SharedKernel.Results;
using DevFlow.SharedKernel.Common;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Update;

internal sealed class UpdateWorklogCommandHandler
    : IRequestHandler<
        UpdateWorklogCommand,
        Result<UpdateWorklogResponse>>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateWorklogCommandHandler(
        IWorklogRepository worklogRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _worklogRepository = worklogRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<UpdateWorklogResponse>> Handle(
        UpdateWorklogCommand request,
        CancellationToken cancellationToken)
    {
        var worklog =
            await _worklogRepository.GetByIdAsync(
                new WorklogId(request.WorklogId),
                cancellationToken);

        if (worklog is null)
        {
            return Result.Failure<UpdateWorklogResponse>(
                WorklogErrors.NotFound);
        }

        if (worklog.UserId != _currentUser.UserId)
        {
            return Result.Failure<UpdateWorklogResponse>(
                WorklogErrors.Forbidden);
        }

        worklog.Update(
            request.Description,
            request.StartedAtUtc,
            request.EndedAtUtc);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateWorklogResponse(
                worklog.Id.Value,
                worklog.WorkItemId,
                worklog.UserId,
                worklog.Description,
                worklog.StartedAtUtc,
                worklog.EndedAtUtc!.Value,
                worklog.MinutesSpent),
            "Worklog updated successfully.");
    }
}
