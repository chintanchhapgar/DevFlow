using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Worklogs.GetAll;

internal sealed class GetAllWorklogsQueryHandler
    : IRequestHandler<
        GetAllWorklogsQuery,
        Result<IReadOnlyList<GetAllWorklogsResponse>>>
{
    private readonly IWorklogRepository _worklogRepository;

    public GetAllWorklogsQueryHandler(
        IWorklogRepository worklogRepository)
    {
        _worklogRepository = worklogRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllWorklogsResponse>>> Handle(
        GetAllWorklogsQuery request,
        CancellationToken cancellationToken)
    {
        var worklogs =
            await _worklogRepository.GetByWorkItemAsync(
                request.WorkItemId,
                cancellationToken);

        var response = worklogs
            .Select(x => new GetAllWorklogsResponse(
                x.Id.Value,
                x.WorkItemId,
                x.UserId,
                x.Description,
                x.StartedAtUtc,
                x.EndedAtUtc,
                x.MinutesSpent,
                x.IsRunning))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyList<GetAllWorklogsResponse>>(
            response,
            "Worklogs retrieved successfully.");
    }
}
