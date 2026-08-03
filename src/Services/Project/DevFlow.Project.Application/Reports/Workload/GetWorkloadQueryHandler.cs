using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.Workload;

internal sealed class GetWorkloadQueryHandler
    : IRequestHandler<
        GetWorkloadQuery,
        Result<GetWorkloadResponse>>
{
    private readonly IWorkloadRepository _repository;

    public GetWorkloadQueryHandler(
        IWorkloadRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetWorkloadResponse>> Handle(
        GetWorkloadQuery request,
        CancellationToken cancellationToken)
    {
        var report = await _repository.GetAsync(
            request.ProjectId,
            cancellationToken);

        if (report is null)
        {
            return Result.Failure<GetWorkloadResponse>(
                ProjectErrors.NotFound);
        }

        return Result.Success(report);
    }
}
