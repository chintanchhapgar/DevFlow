using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.ProjectSummary;

internal sealed class GetProjectSummaryQueryHandler
    : IRequestHandler<
        GetProjectSummaryQuery,
        Result<GetProjectSummaryResponse>>
{
    private readonly IProjectReportRepository _repository;

    public GetProjectSummaryQueryHandler(
        IProjectReportRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetProjectSummaryResponse>> Handle(
        GetProjectSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var summary = await _repository.GetProjectSummaryAsync(
            request.ProjectId,
            cancellationToken);

        if (summary is null)
        {
            return Result.Failure<GetProjectSummaryResponse>(
                ProjectErrors.NotFound);
        }

        return Result.Success(summary);
    }
}
