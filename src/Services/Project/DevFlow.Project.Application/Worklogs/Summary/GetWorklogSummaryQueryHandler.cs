using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.Summary;

internal sealed class GetWorklogSummaryQueryHandler
    : IRequestHandler<
        GetWorklogSummaryQuery,
        Result<GetWorklogSummaryResponse>>
{
    private readonly IWorklogRepository _repository;
    private readonly ICurrentUser _currentUser;

    public GetWorklogSummaryQueryHandler(
        IWorklogRepository repository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Result<GetWorklogSummaryResponse>> Handle(
        GetWorklogSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var worklogs =
            await _repository.GetByWorkItemAsync(
                request.WorkItemId,
                cancellationToken);

        var totalMinutes =
            worklogs.Sum(x => x.MinutesSpent);

        var running =
            worklogs.Any(x =>
                x.IsRunning &&
                x.UserId == _currentUser.UserId);

        return Result.Success(
            new GetWorklogSummaryResponse(
                request.WorkItemId,
                totalMinutes,
                Math.Round(totalMinutes / 60m, 2),
                worklogs.Count,
                running),
            "Summary retrieved successfully.");
    }
}
